using RedditMockup.Common.Dtos;

namespace RedditMockup.Common.Profiles;

public class ProfileProfile : AutoMapper.Profile
{
    public ProfileProfile()
    {
        CreateMap<ProfileDto, Model.Entities.Profile>(AutoMapper.MemberList.None)
        .ForMember(destination => destination.Bio,
            option =>
                option.MapFrom(source => source.Bio))
        .ForMember(destination => destination.Email,
            option =>
                option.MapFrom(source => source.Email))
        .ReverseMap();
    }
}