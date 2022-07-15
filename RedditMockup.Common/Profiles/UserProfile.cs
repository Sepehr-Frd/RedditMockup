using RedditMockup.Common.Dtos;
using RedditMockup.Common.ViewModels;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Profiles;

public class UserProfile : AutoMapper.Profile
{
	public UserProfile()
	{
        CreateMap<User, UserViewModel>(AutoMapper.MemberList.None)
            .ForMember(destination => destination.PersonFullName,
                option =>
                    option.MapFrom(source => source.Person!.FullName))
            .ForMember(destination => destination.Roles,
                option =>
                    option.MapFrom(source => source.UserRoles!.Select(x => x.Role!.Title)))
            .ReverseMap();

        CreateMap<User, UserDto>(AutoMapper.MemberList.None)
			.ForMember(destination => destination.Name,
			option => option.MapFrom(source => source.Person!.Name))
			.ForMember(destination => destination.Family,
			option => option.MapFrom(source => source.Person!.Family))
			.ForMember(destination => destination.Username,
			option => option.MapFrom(source => source.Username))
			.ForMember(destination => destination.Password,
			option => option.MapFrom(source => source.Password))
			.ReverseMap();

	}
}