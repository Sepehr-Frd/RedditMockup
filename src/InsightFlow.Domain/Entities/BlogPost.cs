using InsightFlow.Domain.Common;

namespace InsightFlow.Domain.Entities;

public class BlogPost : DomainEntity
{
    public required string Title { get; set; }

    public required string Body { get; set; }

    public User? Author { get; set; }

    public long AuthorId { get; set; }

    public Image? CoverImage { get; set; }
}