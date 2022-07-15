using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Profiles;

public class VoteProfile : AutoMapper.Profile
{
    public VoteProfile()
    {
        CreateMap<VoteDto, AnswerVote>(AutoMapper.MemberList.None)
            .ForMember(destination => destination.Kind,
                option =>
                    option.MapFrom(source => source.Kind == "Upvote"))
            .ReverseMap();

        CreateMap<VoteDto, QuestionVote>(AutoMapper.MemberList.None)
            .ForMember(destination => destination.Kind,
                option =>
                    option.MapFrom(source => source.Kind == "Upvote"))
            .ReverseMap();
    }
}