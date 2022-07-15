using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Profiles;

public class AnswerProfile : AutoMapper.Profile
{
    public AnswerProfile()
    {
        CreateMap<Answer, AnswerDto>(AutoMapper.MemberList.None)
            .ForMember(destination => destination.Title,
                option =>
                    option.MapFrom(source => source.Title))
            .ForMember(destination => destination.Description,
                option =>
                    option.MapFrom(source => source.Description))
            .ReverseMap();
    }
}