using RedditMockup.Common.Contracts;

namespace RedditMockup.Common.Dtos;

public class QuestionDto : IBaseDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }

}