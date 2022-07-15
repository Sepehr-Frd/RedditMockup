using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Role : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string? Title { get; set; }

    [Sieve(CanSort = true, CanFilter = true)]
    public string? Description { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}