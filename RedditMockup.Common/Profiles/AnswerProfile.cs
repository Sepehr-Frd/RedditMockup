using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Profiles;

public class AnswerProfile : AutoMapper.Profile
{
    public AnswerProfile()
    {
        CreateMap<Answer, AnswerDto>()
            .ReverseMap();
    }
}