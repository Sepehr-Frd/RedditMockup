using Microsoft.EntityFrameworkCore;
using RedditMockup.Common.Helpers;
using RedditMockup.Model.Entities;

namespace RedditMockup.DataAccess.Context;

public class RedditMockupContext : DbContext
{
    #region [Constructor]

    public RedditMockupContext(DbContextOptions options) : base(options) { }

    #endregion

    #region [Properties]

    public DbSet<Answer>? Answers { get; set; }

    public DbSet<Person>? Persons { get; set; }

    public DbSet<Profile>? Profiles { get; set; }

    public DbSet<Question>? Questions { get; set; }

    public DbSet<Role>? Roles { get; set; }

    public DbSet<User>? Users { get; set; }

    public DbSet<UserRole>? UserRoles { get; set; }

    public DbSet<AnswerVote>? Votes { get; set; }

    #endregion

    #region [Methods]

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(new List<Role>
        {
            new()
            {
                Id = 1,
                Title = "Admin",
                Description = "Admin of Application"
            },
            new()
            {
                Id = 2,
                Title = "User",
                Description = "User of Application",
            }
        });

        modelBuilder.Entity<Person>().HasData(new List<Person>
        {
            new()
            {
                Id = 1,
                Name = "Mahyar",
                Family = "Hoorbakht"
            },
            new()
            {
                Id = 2,
                Name = "Sepehr",
                Family = "Foroughi Rad"
            }
        });

        modelBuilder.Entity<User>().HasData(new List<User>
        {
            new()
            {
                Id = 1,
                Username = "admin_admin",
                Password = "adminnnn".GetHashStringAsync().Result,
                PersonId = 1
            },
            new()
            {
                Id = 2,
                Username = "sepehr_frd",
                Password = "sfr1376".GetHashStringAsync().Result,
                PersonId = 2
            }

        });

        modelBuilder.Entity<Profile>().HasData(new List<Profile> {
            new()
            {
                Id = 1,
                UserId = 1,
            },
            new()
            {
                Id = 2,
                UserId = 2,
            }
        });

        modelBuilder.Entity<UserRole>().HasData(new List<UserRole>
        {
            new()
            {
                Id = 1,
                UserId = 1,
                RoleId = 1
            },
            new()
            {
                Id = 2,
                UserId = 2,
                RoleId = 2
            }
        });
    }

    #endregion
}
