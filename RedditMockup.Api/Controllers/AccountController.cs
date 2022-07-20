using Microsoft.AspNetCore.Mvc;
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

    public AccountController(AccountBusiness accountBusiness) =>
        _accountBusiness = accountBusiness;

    [Authorization]
    [HttpGet]
    public async Task<List<UserViewModel>> GetAllUsersAsync([FromQuery] SieveModel sieveModel, CancellationToken cancellationToken) =>
        await _accountBusiness.LoadAllUsersViewModelAsync(sieveModel, cancellationToken);

    [HttpGet]
    [Route("Logout")]
    public async Task<SamanSalamatResponse> Logout() =>
        await AccountBusiness.LogoutAsync(HttpContext);

    [HttpPost]
    [Route("Login")]
    public async Task<SamanSalamatResponse> LoginAsync([FromQuery] LoginDto login, CancellationToken cancellationToken) =>
       await _accountBusiness.LoginAsync(login, HttpContext, cancellationToken);

}