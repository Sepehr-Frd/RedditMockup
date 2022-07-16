using Microsoft.EntityFrameworkCore;
using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class QuestionRepository : BaseRepository<Question>
{
    private readonly RedditMockupContext _context;

    public QuestionRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) : base(context, sieveProcessor) =>
        _context = context;

    public async Task<Question?> GetByIdAsync(int id, CancellationToken cancellationToken = new()) =>
        (await _context.Questions!
            .Include(question => question.User)
            .Include(question => question.Votes)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken))!;
}