﻿using Microsoft.EntityFrameworkCore;
using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class UserRepository : BaseRepository<User>
{
    // [Fields]

    private readonly DbSet<UserRole> _userRoles;
    private readonly DbSet<Bookmark> _bookmarks;

    // --------------------------------------

    // [Constructor]

    public UserRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) :
        base(context, sieveProcessor)
    {
        _userRoles = context.Set<UserRole>();
        _bookmarks = context.Set<Bookmark>();
    }

    // --------------------------------------

    // [Methods]

    public async Task<bool> CreateUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _userRoles.AddAsync(userRole, cancellationToken);

        return entityEntry?.Entity is not null;
    }

    public bool DeleteUserRole(UserRole userRole)
    {
        var entityEntry = _userRoles.Remove(userRole);

        return entityEntry?.Entity is not null;
    }

    public async Task<bool> CreateBookmarkAsync(Bookmark bookmark, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _bookmarks.AddAsync(bookmark, cancellationToken);

        return entityEntry?.Entity is not null;
    }

    public bool DeleteBookmark(Bookmark bookmark)
    {
        var entityEntry = _bookmarks.Remove(bookmark);

        return entityEntry?.Entity is not null;
    }

    public async Task<List<Role?>> GetRolesAsync(int userId, CancellationToken cancellationToken = default) =>
        await _userRoles
            .Where(x => x.UserId == userId)
            .Include(x => x.Role)
            .Select(x => x.Role)
            .ToListAsync(cancellationToken);

    // --------------------------------------
}