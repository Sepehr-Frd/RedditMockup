using RedditMockup.Common.Contracts;
using RedditMockup.Common.Dtos;
using Sieve.Models;

namespace RedditMockup.Business.Contracts;

public interface IBaseBusiness<DTO> where DTO : IBaseDto
{
    Task<SamanSalamatResponse?> CreateAsync(DTO dto, CancellationToken cancellationToken);

    Task<SamanSalamatResponse<List<DTO>>?> LoadAllAsync(SieveModel sieveModel, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> UpdateAsync(DTO dto, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken);
}
