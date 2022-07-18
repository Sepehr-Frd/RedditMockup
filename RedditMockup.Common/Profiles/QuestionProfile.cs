using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Profiles;

public class QuestionProfile : AutoMapper.Profile
{
    public QuestionProfile()
    {
        CreateMap<Question, QuestionDto>()
            .ReverseMap();
    }
}