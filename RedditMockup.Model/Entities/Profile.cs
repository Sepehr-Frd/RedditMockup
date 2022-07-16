using System.ComponentModel.DataAnnotations.Schema;
using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Profile : BaseEntity
{
    public string? Bio { get; set; } = string.Empty; 

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Email { get; set; } = string.Empty;

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}