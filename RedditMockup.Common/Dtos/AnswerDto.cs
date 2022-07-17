using RedditMockup.Common.Contracts;

namespace RedditMockup.Common.Dtos;

public class AnswerDto : IBaseDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int QuestionId { get; set; }
}