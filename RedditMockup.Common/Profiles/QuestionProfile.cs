using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Profiles;

public class QuestionProfile : AutoMapper.Profile
{
    public QuestionProfile()
    {
        CreateMap<Question, QuestionDto>(AutoMapper.MemberList.None)
            .ForMember(destination => destination.Title,
                option =>
                    option.MapFrom(source => source.Title))
            .ForMember(destination => destination.Description,
                option =>
                    option.MapFrom(source => source.Description))
            .ReverseMap();
    }
}