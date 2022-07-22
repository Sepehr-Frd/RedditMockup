using System.ComponentModel.DataAnnotations.Schema;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class User : BaseEntity
{

    #region [Properties]

    [Sieve(CanSort = true, CanFilter = true)]
    public string? Username { get; set; }

    public string? Password { get; set; }

    public int PersonId { get; set; }

    public int Score { get; set; } = 0;


    [ForeignKey("PersonId")]
    [Sieve(CanSort = true, CanFilter = true)]
    public virtual Person? Person { get; set; }

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<Question>? Questions { get; set; }

    public virtual ICollection<Answer>? Answers { get; set; }

    public virtual ICollection<UserRole>? UserRoles { get; set; }

    #endregion

}