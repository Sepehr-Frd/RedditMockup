using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RedditMockup.Common.Dtos;
using RedditMockup.Common.Helpers;
using RedditMockup.Common.ViewModels;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.Businesses;

public class AccountBusiness
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;

    public AccountBusiness(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.UserRepository!;
        _mapper = mapper;
    }

    public async Task<List<UserViewModel>> LoadAllUsersViewModelAsync(SieveModel sieveModel, CancellationToken cancellationToken = new()) =>
            _mapper.Map<List<UserViewModel>>(await _userRepository.LoadAllAsync(sieveModel,
                include => include
                    .Include(x => x.Person)
                    .Include(x => x.UserRoles)!
                    .ThenInclude(x => x.Role),
                cancellationToken));

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = new()) =>
        !await _userRepository.UsernameExistsAsync(username, cancellationToken);

    public async Task<bool> IsUsernameAndPasswordValidAsync(LoginDto login, CancellationToken cancellationToken = new()) =>
        await _userRepository.IsUsernameAndPasswordValidAsync(login.Username!, await login.Password!.GetHashStringAsync(), cancellationToken);

    public async Task<User> LoadByUsernameAsync(string username, CancellationToken cancellationToken = new()) =>
        await _userRepository.LoadByUsernameAsync(username, cancellationToken);

    private static bool IsSignedIn(HttpContext httpContext) =>
        httpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) != null;

    public async Task<SamanSalamatResponse> LoginAsync(LoginDto login, HttpContext httpContext, CancellationToken cancellationToken = new())
    {
        if (IsSignedIn(httpContext))
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "You are already signed in!"
            };
        }

        var isValid = await IsUsernameAndPasswordValidAsync(login, cancellationToken);

        if (!isValid)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "Username and/or password not correct!"
            };

        }

        var user = await LoadByUsernameAsync(login.Username!, cancellationToken);

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("FullName", user.Person!.FullName),
                new("Username", user.Username!)
            };

        var roles = await _unitOfWork.RoleRepository!.LoadByUserIdAsync(user.Id, cancellationToken);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role!.Title!)));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties
        {
            IsPersistent = login.RememberMe
        };

        await httpContext.SignInAsync(principal, properties);

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = "Successfully signed in."
        }; ;
    }

    public async Task<SamanSalamatResponse> LogoutAsync(HttpContext httpContext)
    {

        if (!IsSignedIn(httpContext))
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "You are not signed in!"
            };
        }

        await httpContext.SignOutAsync();

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = "Successfully signed out."
        };
    }
}