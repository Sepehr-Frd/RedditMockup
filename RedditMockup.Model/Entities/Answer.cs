using System.ComponentModel.DataAnnotations.Schema;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Answer : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string? Title { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Description { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int QuestionId { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int? UserId { get; set; }


    public virtual ICollection<AnswerVote> Votes { get; set; } = new List<AnswerVote>();

    [ForeignKey("QuestionId")]
    public virtual Question? Question { get; set; }

    [ForeignKey("UserId")]
    public virtual User? AnsweringUser { get; set; }
}