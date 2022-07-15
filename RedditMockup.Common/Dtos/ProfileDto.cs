using RedditMockup.Common.Contracts;

namespace RedditMockup.Common.Dtos;

public class ProfileDto : IBaseDto
{
    public string? Bio { get; set; }
    public string? Email { get; set; }
}