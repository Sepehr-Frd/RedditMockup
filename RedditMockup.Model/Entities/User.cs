using Sieve.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedditMockup.Model.Entities;

public class User : BaseEntity
{

    [Sieve(CanSort = true, CanFilter = true)]
    public string? Username { get; set; }

    public string? Password { get; set; }

    public int PersonId { get; set; }

    public int Score { get; set; } = 0;

    [Sieve(CanSort = true, CanFilter = true)]
    [ForeignKey("PersonId")]
    public virtual Person? Person { get; set; }

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<Question>? Questions { get; set; }

    public virtual ICollection<Answer>? Answers { get; set; }

    public virtual ICollection<UserRole>? UserRoles { get; set; }
}