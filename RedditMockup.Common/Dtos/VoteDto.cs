using RedditMockup.Common.Contracts;

namespace RedditMockup.Common.Dtos;

public class VoteDto : IBaseDto
{
    public string Kind { get; set; }
}