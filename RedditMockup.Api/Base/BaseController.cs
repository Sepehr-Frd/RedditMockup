using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditMockup.Api.Contracts;
using RedditMockup.Api.Filters;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Api.Base;

[ApiController]
[Route("api/[controller]")]
[Authorization]
public class BaseController<T, DTO> : ControllerBase, IBaseController<DTO>
    where T : BaseEntity
{
    private readonly IBaseBusiness<T, DTO> _business;

    public BaseController(IBaseBusiness<T, DTO> business) =>
        _business = business;

    [HttpGet]
    [AllowAnonymous]
    public async Task<SamanSalamatResponse<IEnumerable<DTO>>?> GetAllAsync([FromQuery] SieveModel sieveModel, CancellationToken cancellationToken) =>
        await _business.LoadAllAsync(sieveModel, cancellationToken);

    [Route("id")]
    [HttpGet]
    public async Task<SamanSalamatResponse?> GetByIdAsync([FromQuery] int id, CancellationToken cancellationToken) =>
        await _business.LoadByIdAsync(id, cancellationToken);

    [HttpPost]
    public async Task<SamanSalamatResponse?> CreateAsync([FromQuery] DTO dto, CancellationToken cancellationToken) =>
        await _business.CreateAsync(dto, HttpContext, cancellationToken);

    [HttpDelete]
    public async Task<SamanSalamatResponse?> DeleteAsync([FromQuery] int id, CancellationToken cancellationToken) =>
        await _business.DeleteAsync(id, cancellationToken);

    async Task<SamanSalamatResponse?> IBaseController<DTO>.UpdateAsync(int id, DTO dto, CancellationToken cancellationToken) =>
        await _business.UpdateAsync(id, dto, cancellationToken);

    [HttpOptions]
    public void Options() =>
      Response.Headers.Add("Allow", "POST,PUT,DELETE,GET");

}