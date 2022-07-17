using RedditMockup.Common.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.Contracts;

public interface IBaseBusiness<T, DTO>
    where DTO : IBaseDto
    where T : BaseEntity
{
    Task<SamanSalamatResponse?> CreateAsync(DTO dto, CancellationToken cancellationToken);

    Task<SamanSalamatResponse<List<DTO>>?> LoadAllAsync(SieveModel sieveModel, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> UpdateAsync(DTO dto, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken);
}
