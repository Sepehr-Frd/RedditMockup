using InsightFlow.Application.Interfaces.Repositories;

namespace InsightFlow.Application.Interfaces;

public interface IUnitOfWork
{
    IBlogPostRepository BlogPostRepository { get; }

    IUserRepository UserRepository { get; }

    IRoleRepository RoleRepository { get; }

    IUserRoleRepository UserRoleRepository { get; }

    IImageRepository ImageRepository { get; }

    int CommitChanges();

    Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);
}