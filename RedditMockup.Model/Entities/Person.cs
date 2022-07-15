using Sieve.Attributes;

namespace RedditMockup.Model.Entities;

public class Person : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string? Name { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Family { get; set; }

    public string FullName => Name + " " + Family;

    public virtual User? User { get; set; }
}