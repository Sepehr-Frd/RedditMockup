using System.ComponentModel.DataAnnotations.Schema;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Profile : BaseEntity
{
    public string? Bio { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Email { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}