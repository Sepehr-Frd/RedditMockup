using InsightFlow.Domain.Common;

namespace InsightFlow.Domain.Entities;

public class User : DomainEntity
{
    public required string Username { get; set; }

    public required string PasswordHash { get; set; }

    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public Image? ProfileImage { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = [];

    public ICollection<BlogPost> BlogPosts { get; set; } = [];
}