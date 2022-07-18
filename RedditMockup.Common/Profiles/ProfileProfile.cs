using RedditMockup.Common.Dtos;

namespace RedditMockup.Common.Profiles;

public class ProfileProfile : AutoMapper.Profile
{
    public ProfileProfile()
    {
        CreateMap<Model.Entities.Profile, ProfileDto>()
        .ReverseMap();
    }
}