using RedditMockup.Common.Dtos;
using Sieve.Models;
using System.Collections;

namespace RedditMockup.Api.Contracts;

public interface IBaseController<DTO>

{
    Task<SamanSalamatResponse?> CreateAsync(DTO dto, CancellationToken cancellationToken);

    Task<SamanSalamatResponse<IEnumerable>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> UpdateAsync(int id, DTO dto, CancellationToken cancellationToken);

    Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken);

    void Options();
}


/*
 * GetAllAsync(sievemodel) //Admin
 * GetByIdAsync(int id) Admin
 * AddAsync(T) Admin
 * Update(T) Admin
 * Delete(int) Admin
 *
 */