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
using System.Security.Claims;

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

    private async Task<bool> IsUsernameAndPasswordValidAsync(LoginDto login, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new()
        {
            Filters = $"Username=={login.Username!}"
        };

        var users = await _userRepository.LoadAllAsync(sieveModel, null, cancellationToken);

        if (users.Count == 0)
        {
            return false;
        }

        var isPasswordValid = (await login.Password!.GetHashStringAsync()) == users.Single().Password;

        return isPasswordValid;
    }

    private static bool IsSignedIn(HttpContext httpContext) =>
        httpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) is not null;

    private async Task<User?> LoadByUsernameAsync(string username, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new()
        {
            Filters = $"Username=={username}"
        };

        var users = await _userRepository.LoadAllAsync(sieveModel, include =>
        include
        .Include(x => x.Person)
        .Include(x => x.Profile)
        .Include(x => x.Questions)
        .Include(x => x.Answers)
        .Include(x => x.UserRoles),
        cancellationToken);

        if (users.Count == 0)
        {
            return null;
        }

        return users.Single();
    }

    public async Task<List<UserViewModel>> LoadAllUsersViewModelAsync(SieveModel sieveModel, CancellationToken cancellationToken = new()) =>
            _mapper.Map<List<UserViewModel>>(await _userRepository.LoadAllAsync(sieveModel,
            include => include
                .Include(x => x.Person)
                .Include(x => x.UserRoles)!
                .ThenInclude(x => x.Role),
            cancellationToken));

    public async Task<SamanSalamatResponse> LoginAsync(LoginDto login, HttpContext httpContext, CancellationToken cancellationToken = new())
    {
        if (IsSignedIn(httpContext))
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "You are already signed in"
            };
        }

        var isValid = await IsUsernameAndPasswordValidAsync(login, cancellationToken);

        if (!isValid)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "Username and/or password not correct"
            };
        }

        var user = await LoadByUsernameAsync(login.Username!, cancellationToken);

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user!.Id.ToString()),
                new("FullName", user.Person!.FullName),
                new("Username", user.Username!)
            };

        var roles = await _unitOfWork.RoleRepository!.LoadByUserIdAsync(user.Id, cancellationToken);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role!.Title!)));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var properties = new AuthenticationProperties()
        {
            IsPersistent = login.RememberMe
        };

        await httpContext.SignInAsync(principal, properties);

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = "Successfully logged in"
        };
    }

    public static async Task<SamanSalamatResponse> LogoutAsync(HttpContext httpContext)
    {

        if (!IsSignedIn(httpContext))
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "Already logged out"
            };
        }

        await httpContext.SignOutAsync();

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = "Successfully logged out"
        };
    }

}