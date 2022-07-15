using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RedditMockup.Api.Filters;
using RedditMockup.Business.Businesses;
using RedditMockup.Common.Dtos;
using RedditMockup.Common.ViewModels;
using Sieve.Models;

namespace RedditMockup.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountBusiness _accountBusiness;

    private readonly UserBusiness _userBusiness;

    private readonly ProfileBusiness _profileBusiness;

    public AccountController(AccountBusiness accountBusiness, UserBusiness userBusiness, ProfileBusiness profileBusiness)
    {
        _accountBusiness = accountBusiness;
        _userBusiness = userBusiness;
        _profileBusiness = profileBusiness;
    }

    [HttpPost]
    [Route("Signup")]
    [AllowAnonymous]
    public async Task<SamanSalamatResponse> CreateAccountAsync([FromQuery] UserDto signup, CancellationToken cancellationToken) =>
        (await _userBusiness.CreateAsync(signup, cancellationToken))!;


    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<SamanSalamatResponse> LoginAsync([FromQuery] LoginDto login, CancellationToken cancellationToken) =>
        await _accountBusiness.LoginAsync(login, HttpContext, cancellationToken);

    [HttpGet]
    public async Task<List<UserViewModel>> GetAllUsersAsync([FromQuery] SieveModel sieveModel, CancellationToken cancellationToken) =>
        await _accountBusiness.LoadAllUsersViewModelAsync(sieveModel, cancellationToken);

    [HttpGet]
    [Route("Logout")]
    public async Task<SamanSalamatResponse> Logout() =>
        await _accountBusiness.LogoutAsync(HttpContext);

    [Authorization]
    [HttpPost]
    [Route("UpdateProfile")]
    public async Task<SamanSalamatResponse?> UpdateProfileAsync([FromQuery] ProfileDto profileDto, CancellationToken cancellationToken) =>
        await _profileBusiness.UpdateAsync(profileDto, HttpContext, cancellationToken);

}