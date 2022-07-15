using System.ComponentModel.DataAnnotations.Schema;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class AnswerVote : BaseEntity
{
    [Sieve(CanSort = true)]
    public bool Kind { get; set; }

    public int AnswerId { get; set; }

    [ForeignKey("AnswerId")]
    public virtual Answer? Answer { get; set; }

}

public class QuestionVote : BaseEntity
{
    [Sieve(CanSort = true)]
    public bool Kind { get; set; }

    public int QuestionId { get; set; }

    [ForeignKey("QuestionId")]
    public virtual Question? Question { get; set; }

}