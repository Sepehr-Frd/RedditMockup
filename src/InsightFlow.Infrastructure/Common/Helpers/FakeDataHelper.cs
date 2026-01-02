using Bogus;
using InsightFlow.Domain.Common;
using InsightFlow.Domain.Entities;

namespace InsightFlow.Infrastructure.Common.Helpers;

public static class FakeDataHelper
{
    private static User? FakeAdmin;

    private static User? FakeUser;

    public static List<BlogPost> GetFakeBlogPosts(List<User> fakeUsers, int countToGenerate)
    {
        var blogPostFaker = new Faker<BlogPost>()
            .RuleFor(blogPost => blogPost.Title, faker => faker.Lorem.Word())
            .RuleFor(blogPost => blogPost.Body, faker => faker.Lorem.Paragraph())
            .RuleFor(blogPost => blogPost.Author, faker => faker.PickRandom(fakeUsers));

        var fakeBlogPosts = blogPostFaker.Generate(countToGenerate);

        return fakeBlogPosts;
    }

    public static List<User> GetFakeUsers(int countToGenerate)
    {
        var password = PasswordHelper.HashPassword("Correct_p0");

        var userFaker = new Faker<User>()
            .RuleFor(user => user.Username, faker => $"sample_username{faker.IndexFaker}")
            .RuleFor(user => user.PasswordHash, _ => password)
            .RuleFor(user => user.Email, faker => faker.Internet.Email().ToLowerInvariant())
            .RuleFor(user => user.FirstName, faker => faker.Name.FirstName())
            .RuleFor(user => user.LastName, faker => faker.Name.LastName());

        var fakeUsers = new List<User>();

        FakeAdmin = new User
        {
            Uuid = Guid.NewGuid(),
            Username = "sepehr_frd",
            PasswordHash = PasswordHelper.HashPassword("Sfr1376."),
            Email = "sepfrd@outlook.com",
            FirstName = "Sepehr",
            LastName = "Foroughi Rad"
        };

        FakeUser = new User
        {
            Uuid = Guid.NewGuid(),
            Username = "bernard_cool",
            PasswordHash = PasswordHelper.HashPassword("BernardCool1997."),
            Email = "bercool@gmail.com",
            FirstName = "Bernard",
            LastName = "Cool"
        };

        fakeUsers.AddRange(new List<User>
        {
            FakeAdmin,
            FakeUser
        });

        var currentUsersCount = fakeUsers.Count;

        fakeUsers.AddRange(userFaker.Generate(countToGenerate - currentUsersCount));

        return fakeUsers;
    }

    public static List<UserRole> GetFakeUserRoles(List<User> fakeUsers, List<Role> fakeRoles)
    {
        var userRolesList = new List<UserRole>
        {
            new()
            {
                User = fakeUsers.First(user => user.Username == FakeAdmin!.Username),
                Role = fakeRoles.First(role => role.Title == DomainConstants.BasicUserRoleTitle)
            },
            new()
            {
                User = fakeUsers.First(user => user.Username == FakeAdmin!.Username),
                Role = fakeRoles.First(role => role.Title == DomainConstants.AdminRoleTitle)
            },
            new()
            {
                User = fakeUsers.First(user => user.Username == FakeUser!.Username),
                Role = fakeRoles.First(role => role.Title == DomainConstants.BasicUserRoleTitle)
            }
        };

        var currentUserRolesListCount = userRolesList.Count;

        userRolesList.AddRange(fakeUsers[currentUserRolesListCount..]
            .Select(user =>
                new UserRole
                {
                    User = user,
                    Role = fakeRoles[Random.Shared.Next(0, fakeRoles.Count)]
                }));

        return userRolesList;
    }
}