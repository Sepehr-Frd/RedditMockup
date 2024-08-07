﻿using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Constants;
using RedditMockup.Common.Dtos;
using RedditMockup.Common.Helpers;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.DomainEntityBusinesses;

public class AuthBusiness : IAuthBusiness
{
    private readonly UserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public async Task<CustomResponse<string>> LoginAsync(LoginDto login, CancellationToken cancellationToken = default)
    {
        if (IsSignedIn())
        {
            return CustomResponse<string>.CreateUnsuccessfulResponse(HttpStatusCode.BadRequest, MessageConstants.AlreadySignedInMessage);
        }

        var user = await ValidateAndGetUserByCredentialsAsync(login, cancellationToken);

        if (user is null)
        {
            return CustomResponse<string>.CreateUnsuccessfulResponse(HttpStatusCode.BadRequest, MessageConstants.InvalidCredentialsMessage);
        }

        var jwt = CreateJwt(user)!;

        return CustomResponse<string>.CreateSuccessfulResponse(jwt, MessageConstants.SuccessfulLoginMessage);
    }

    public AuthBusiness(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _userRepository = (UserRepository)unitOfWork.UserRepository!;
    }

    private async Task<User?> ValidateAndGetUserByCredentialsAsync(LoginDto loginDto,
        CancellationToken cancellationToken = default)
    {
        var user = await LoadByUsernameAsync(loginDto.Username!, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var password = await loginDto.Password!.GetHashStringAsync();

        var isPasswordValid = password == user.Password;

        return !isPasswordValid ? null : user;
    }

    private bool IsSignedIn() =>
        _httpContextAccessor.HttpContext!.User.Identity is not null && _httpContextAccessor.HttpContext!.User.Identity.IsAuthenticated;

    private async Task<User?> LoadByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        SieveModel sieveModel = new()
        {
            Filters = nameof(User.Username) + "==" + username
        };

        var users = await _userRepository.GetAllAsync(sieveModel,
            users => users.Include(x => x.Person)
                .Include(x => x.Profile)
                .Include(x => x.Questions)
                .Include(x => x.Answers)
                .Include(x => x.UserRoles)!
                .ThenInclude(x => x.Role), cancellationToken);

        return users.Count == 0 ? null : users.Single();
    }

    private string? CreateJwt(User user)
    {
        var serverUrl = _configuration
            .GetSection(ApplicationConstants.ApplicationUrlsConfigurationSectionKey)
            .GetValue<string>(ApplicationConstants.ServerUrlConfigurationKey)!;

        var clientUrl = _configuration.GetSection(ApplicationConstants.ApplicationUrlsConfigurationSectionKey)
            .GetValue<string>(ApplicationConstants.ClientUrlConfigurationKey)!;

        var privateKey = _configuration
            .GetSection(ApplicationConstants.JwtConfigurationSectionKey)
            .GetValue<string>(ApplicationConstants.JwtPrivateKeyConfigurationKey);

        var rsa = RSA.Create();

        rsa.ImportFromPem(privateKey);

        var securityKey = new RsaSecurityKey(rsa);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512Signature);

        var claims = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Iss, serverUrl),
            new Claim(JwtRegisteredClaimNames.Aud, clientUrl),
            new Claim(
                JwtRegisteredClaimNames.Iat,
                DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture)),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Profile!.Email),
            new Claim(ApplicationConstants.UsernameClaim, user.Username),
            new Claim(ApplicationConstants.ExternalIdClaim, user.Guid.ToString())
        });

        var roles = user.UserRoles!.Select(userRole => userRole.Role).ToList();

        foreach (var role in roles)
        {
            var claim = new Claim(ClaimTypes.Role, role?.Title!);

            claims.AddClaim(claim);
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = signingCredentials
        };

        var jwtHandler = new JwtSecurityTokenHandler();

        var token = jwtHandler.CreateToken(tokenDescriptor);

        var jwt = jwtHandler.WriteToken(token);

        return jwt;
    }
}