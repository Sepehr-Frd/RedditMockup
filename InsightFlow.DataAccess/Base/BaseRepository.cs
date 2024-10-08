﻿using InsightFlow.DataAccess.Dtos;
using InsightFlow.DataAccess.Interfaces;
using InsightFlow.Model.Entities;
using InsightFlow.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Sieve.Models;
using Sieve.Services;

namespace InsightFlow.DataAccess.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;
    private readonly ISieveProcessor _processor;

    public BaseRepository(InsightFlowDbContext dbContext, ISieveProcessor processor)
    {
        _processor = processor;
        _dbSet = dbContext.Set<T>();
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _dbSet.AddAsync(entity, cancellationToken);

        return entityEntry.Entity;
    }

    public async Task<PagedEntitiesResponseDto<T>> GetAllAsync(SieveModel sieveModel, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking();

        if (include != null)
        {
            query = include(query);
        }

        var filteredQuery = _processor.Apply(sieveModel, query, applyPagination: false, applySorting: false);

        var totalCount = filteredQuery.Count();

        var sortedQuery = filteredQuery.OrderByDescending(entity => entity.Id);

        var paginatedEntities = await _processor.Apply(sieveModel, sortedQuery, applyFiltering: false).ToListAsync(cancellationToken);

        return new PagedEntitiesResponseDto<T>(paginatedEntities, totalCount);
    }

    public async Task<PagedEntitiesResponseDto<T>> GetAllActiveAsync(SieveModel sieveModel, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().Where(entity => entity.State == BaseEntityState.Active);

        if (include != null)
        {
            query = include(query);
        }

        var filteredQuery = _processor.Apply(sieveModel, query, applyPagination: false, applySorting: false);

        var totalCount = filteredQuery.Count();

        var sortedQuery = filteredQuery.OrderByDescending(entity => entity.Id);

        var paginatedEntities = await _processor.Apply(sieveModel, sortedQuery, applyFiltering: false).ToListAsync(cancellationToken);

        return new PagedEntitiesResponseDto<T>(paginatedEntities, totalCount);
    }

    public async Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(entity => entity.Id == id);

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> GetByGuidAsync(Guid guid, Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(entity => entity.Guid == guid && entity.State == BaseEntityState.Active);

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public void Delete(T entity) =>
        _dbSet.Remove(entity);
}