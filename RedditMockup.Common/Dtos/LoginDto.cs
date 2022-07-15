using RedditMockup.Common.Contracts;

namespace RedditMockup.Common.Dtos;

public class LoginDto : IBaseDto
{

    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool RememberMe { get; set; }

}