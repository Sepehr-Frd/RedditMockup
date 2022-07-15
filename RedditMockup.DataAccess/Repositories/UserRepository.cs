using Microsoft.EntityFrameworkCore;
using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class UserRepository : BaseRepository<User>
{
    private readonly RedditMockupContext _context;

    public UserRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) : base(context, sieveProcessor) =>
        _context = context;

    public async Task<bool> IsUsernameAndPasswordValidAsync(string username, string password, CancellationToken cancellationToken = new()) =>
        await _context.Users!.AnyAsync(x =>
            x.Username == username && x.Password == password, cancellationToken);

    public async Task<User> LoadByUsernameAsync(string username, CancellationToken cancellationToken = new()) =>
        (await _context.Users!
            .Include(x => x.Person)
            .SingleOrDefaultAsync(x => x.Username!.ToLower() == username.ToLower(), cancellationToken))!;

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = new()) =>
        await _context.Users!
            .AnyAsync(x => x.Username!.ToLower() == username.ToLower(),
                cancellationToken);

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = new()) =>
        await _context.Users!
            .Include(x => x.Person)
            .Include(x => x.Profile)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

}