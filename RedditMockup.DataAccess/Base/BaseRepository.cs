using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RedditMockup.DataAccess.Context;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.Model.Entities;
using Sieve.Models;
using Sieve.Services;

namespace RedditMockup.DataAccess.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    #region [Fields]

    private readonly DbSet<T> _dbSet;

    private readonly ISieveProcessor _processor;

    private readonly RedditMockupContext _context;

    #endregion

    #region [Constructor]

    public BaseRepository(RedditMockupContext context, ISieveProcessor processor)
    {
        _processor = processor;
        _dbSet = context.Set<T>();
        _context = context;
    }

    #endregion


    #region [Methods]

    public async Task<T> CreateAsync(T t, CancellationToken cancellationToken = new()) =>
        (await _dbSet.AddAsync(t, cancellationToken)).Entity;

    public async Task<List<T>> LoadAllAsync(SieveModel sieveModel, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
        CancellationToken cancellationToken = new())
    {
        var query = _dbSet.AsNoTracking();
        if (include != null)
            query = include(query);
        return await _processor.Apply(sieveModel, query).ToListAsync(cancellationToken);
    }

    public async Task<T> UpdateAsync(T t, CancellationToken cancellationToken = new())
    {
        var local = _dbSet
            .Local
            .FirstOrDefault(entry => entry.Id.Equals(t.Id));

        if (local is not null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        var updatedT = (await Task.FromResult(_dbSet.Update(t))).Entity;

        _context.Entry(t).State = EntityState.Modified;

        t.LastUpdated = DateTime.Now;

        return updatedT;
    }

    public async Task<T> DeleteAsync(T t, CancellationToken cancellationToken = new()) =>
        (await Task.FromResult(_dbSet.Remove(t))).Entity;

    #endregion
}