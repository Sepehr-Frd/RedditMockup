namespace RedditMockup.Common.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? PersonFullName { get; set; }

    public ICollection<string>? Roles { get; set; }
}