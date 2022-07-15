using RedditMockup.Common.Contracts;

namespace RedditMockup.Common.Dtos;

public class UserDto : IBaseDto
{

    public string? Name { get; set; }

    public string? Family { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

}