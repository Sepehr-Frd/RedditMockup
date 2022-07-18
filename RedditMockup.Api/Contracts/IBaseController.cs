using System.Collections;
using Microsoft.AspNetCore.Http;
using RedditMockup.Common.Dtos;
using Sieve.Models;

namespace RedditMockup.Api.Contracts;

public interface IBaseController<DTO>

{
    Task<SamanSalamatResponse?> CreateAsync(DTO dto, CancellationToken cancellationToken);

    Task<SamanSalamatResponse<IEnumerable>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> UpdateAsync(DTO dto, HttpContext httpContext, CancellationToken cancellationToken);

    
    Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken);

    void Options();
}