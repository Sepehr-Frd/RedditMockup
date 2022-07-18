using System.Collections;
using RedditMockup.Common.Dtos;
using Sieve.Models;

namespace RedditMockup.Business.Contracts;

public interface IBaseBusiness<T>
{
    Task<SamanSalamatResponse?> CreateAsync(T t, CancellationToken cancellationToken);

    Task<SamanSalamatResponse<IEnumerable>?> LoadAllAsync(SieveModel sieveModel, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> UpdateAsync(T t, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> DeleteAsync(T t, CancellationToken cancellationToken);
}