using System.ComponentModel.DataAnnotations.Schema;
using Sieve.Attributes;

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

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}