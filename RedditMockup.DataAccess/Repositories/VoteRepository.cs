using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class AnswerVoteRepository : BaseRepository<AnswerVote>
{
    public AnswerVoteRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) : base(context, sieveProcessor)
    {
    }
}

public class QuestionVoteRepository : BaseRepository<QuestionVote>
{
    public QuestionVoteRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) : base(context, sieveProcessor)
    {
    }
}