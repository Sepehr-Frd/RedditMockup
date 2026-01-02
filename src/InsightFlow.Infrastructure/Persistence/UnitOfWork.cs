using InsightFlow.Application.Interfaces;
using InsightFlow.Application.Interfaces.Repositories;
using InsightFlow.Domain.Entities;
using InsightFlow.Infrastructure.Interfaces;
using InsightFlow.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InsightFlow.Infrastructure.Persistence;

public class UnitOfWork : DbContext, IUnitOfWork
{
    private readonly IDbConnectionPool _dbConnectionPool;
    private BlogPostRepository? _blogPostRepository;
    private UserRepository? _userRepository;
    private RoleRepository? _roleRepository;
    private UserRoleRepository? _userRoleRepository;
    private ImageRepository? _profileImageRepository;

    public UnitOfWork(DbContextOptions dbContextOptions, IDbConnectionPool dbConnectionPool) : base(dbContextOptions)
    {
        _dbConnectionPool = dbConnectionPool;
    }

    public DbSet<BlogPost> BlogPosts { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Image> Images { get; set; }

    public IBlogPostRepository BlogPostRepository => _blogPostRepository ??= new BlogPostRepository(BlogPosts, _dbConnectionPool);

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(Users, _dbConnectionPool);

    public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(Roles, _dbConnectionPool);

    public IUserRoleRepository UserRoleRepository => _userRoleRepository ??= new UserRoleRepository(UserRoles, _dbConnectionPool);

    public IImageRepository ImageRepository => _profileImageRepository ??= new ImageRepository(Images, _dbConnectionPool);

    public int CommitChanges() => SaveChanges();

    public async Task<int> CommitChangesAsync(CancellationToken cancellationToken = default) =>
        await SaveChangesAsync(cancellationToken);
}