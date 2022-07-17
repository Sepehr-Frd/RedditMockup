using Microsoft.EntityFrameworkCore;
using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class AnswerRepository : BaseRepository<Answer>
{
    private readonly RedditMockupContext _context;
    public AnswerRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) : base(context, sieveProcessor) =>
        _context = context;

    public async Task<Answer?> GetByIdAsync(int id, CancellationToken cancellationToken = new()) =>
        await _context.Answers!
            .Include(answer => answer.Votes)
            .Include(answer => answer.AnsweringUser)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
}