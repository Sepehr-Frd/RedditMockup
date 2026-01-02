using InsightFlow.Application.Interfaces.Repositories;
using InsightFlow.Domain.Entities;
using InsightFlow.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InsightFlow.Infrastructure.Persistence.Repositories;

public class ImageRepository : RepositoryBase<Image, long>, IImageRepository
{
    public ImageRepository(DbSet<Image> dbSet, IDbConnectionPool dbConnectionPool)
        : base(dbSet, dbConnectionPool)
    {
    }
}