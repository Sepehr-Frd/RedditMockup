using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditMockup.Api.Contracts;
using RedditMockup.Api.Filters;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Api.Base;

[ApiController]
[Route("api/[controller]")]
[Authorization]
public class BaseController<T, DTO> : ControllerBase, IBaseController<T, DTO>
    where T : BaseEntity
    where DTO : IBaseDto
{

    private readonly IBaseBusiness<DTO> _business;

    public BaseController(IBaseBusiness<DTO> business) =>
        _business = business;


    [HttpGet]
    [AllowAnonymous]
    public async Task<SamanSalamatResponse<List<DTO>>?> GetAllAsync([FromQuery] SieveModel sieveModel, CancellationToken cancellationToken) =>
        await _business.LoadAllAsync(sieveModel, cancellationToken);

    [HttpPost]
    public async virtual Task<SamanSalamatResponse?> CreateAsync([FromQuery] DTO dto, CancellationToken cancellationToken) =>
        await _business.CreateAsync(dto, cancellationToken);

    [HttpPut]
    public async Task<SamanSalamatResponse?> UpdateAsync([FromQuery] DTO dto, CancellationToken cancellationToken) =>
        await _business.UpdateAsync(dto, cancellationToken);

    [HttpDelete]
    public async Task<SamanSalamatResponse?> DeleteAsync([FromQuery] int id, CancellationToken cancellationToken) =>
        await _business.DeleteAsync(id, cancellationToken);

    [HttpOptions]
    public void Options() =>
        Response.Headers.Add("Allow", "POST,PUT,DELETE,GET");
}