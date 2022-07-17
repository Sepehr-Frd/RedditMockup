using RedditMockup.Common.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Api.Contracts;

public interface IBaseController<DTO>
    where DTO : IBaseDto
{
    Task<SamanSalamatResponse?> CreateAsync(DTO dto, CancellationToken cancellationToken);

    Task<SamanSalamatResponse<List<DTO>>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken);

    void Options();
}