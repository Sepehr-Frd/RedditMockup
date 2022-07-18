using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class AnswerRepository : BaseRepository<Answer>
{
    public AnswerRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) : base(context, sieveProcessor)
    {
    }
}