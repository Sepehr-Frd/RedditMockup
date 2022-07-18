using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Profiles;

public class VoteProfile : AutoMapper.Profile
{
    public VoteProfile()
    {
        CreateMap<VoteDto, AnswerVote>()
            .ReverseMap();

        CreateMap<VoteDto, QuestionVote>()
            .ReverseMap();
    }
}