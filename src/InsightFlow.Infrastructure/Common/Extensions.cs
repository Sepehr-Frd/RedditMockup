using FluentValidation;
using InsightFlow.Application.Features.BlogPosts.Commands.UpdateBlogPost;
using InsightFlow.Application.Features.BlogPosts.Dtos;
using InsightFlow.Application.Features.Users.Commands.UpdateUser;
using InsightFlow.Application.Features.Users.Dtos;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Constants;
using InsightFlow.Domain.Common;
using InsightFlow.Domain.Entities;
using InsightFlow.Infrastructure.Common.Configurations;
using InsightFlow.Infrastructure.Common.Constants;
using InsightFlow.Infrastructure.Common.Dtos;
using InsightFlow.Infrastructure.Common.Helpers;
using InsightFlow.Infrastructure.Interfaces;
using InsightFlow.Infrastructure.Persistence;
using InsightFlow.Infrastructure.Services;
using InsightFlow.Infrastructure.Validators;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace InsightFlow.Infrastructure.Common;

public static class Extensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(
            AppOptions appOptions,
            IHostEnvironment environment)
        {
            return services
                .AddDatabase(appOptions, environment)
                .AddSingleton<IMappingService, MappingService>()
                .AddSingleton<IDataValidator<CreateUserRequestDto>, DataValidator<CreateUserRequestDto>>()
                .AddSingleton<IDataValidator<PaginationDto>, DataValidator<PaginationDto>>()
                .AddSingleton<IDataValidator<LoginDto>, DataValidator<LoginDto>>()
                .AddSingleton<IRoleService, RoleService>()
                .AddSingleton<IDbConnectionPool, DbConnectionPool>(serviceProvider =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<AppOptions>>().Value;

                    var connectionString = options.DatabaseProvider == StringConstants.Sqlite
                        ? options.SqliteConnectionString!
                        : options.SqlServerConnectionString!;

                    DbConnectionPool.Initialize(
                        connectionString,
                        options.DatabaseProvider ?? StringConstants.Sqlite);

                    return DbConnectionPool.Instance;
                })
                .AddValidatorsFromAssemblyContaining<CreateUserRequestDtoValidator>(ServiceLifetime.Singleton)
                .AddScoped<IAuthService, AuthService>()
                .AddSingleton(_ =>
                {
                    var config = new TypeAdapterConfig();
                    ConfigureMapster(config);
                    return config;
                })
                .AddSingleton<IMapper>(serviceProvider =>
                    new Mapper(serviceProvider.GetRequiredService<TypeAdapterConfig>()));
        }

        private static void ConfigureMapster(TypeAdapterConfig config)
        {
            config
                .NewConfig<BlogPost, BlogPostResponseDto>()
                .Map(
                    dto => dto.Author,
                    blogPost => blogPost.Author)
                .Map(dto => dto.CreatedAt,
                    blogPost => DateTime.SpecifyKind(blogPost.CreatedAt, DateTimeKind.Utc))
                .Map(dto => dto.UpdatedAt,
                    blogPost => DateTime.SpecifyKind(blogPost.UpdatedAt, DateTimeKind.Utc));

            config
                .NewConfig<UpdateBlogPostCommand, BlogPost>()
                .Map(
                    blogPost => blogPost.Title,
                    updateCommand => updateCommand.NewTitle)
                .Map(
                    blogPost => blogPost.Body,
                    updateCommand => updateCommand.NewBody);

            config.NewConfig<User, UserResponseDto>()
                .Map(
                    dto => dto.FullName,
                    user => user.FirstName + ' ' + user.LastName);

            config.NewConfig<UpdateUserCommand, User>()
                .Map(
                    user => user.FirstName,
                    updateUserCommand => updateUserCommand.NewFirstName)
                .Map(
                    user => user.LastName,
                    updateUserCommand => updateUserCommand.NewLastName)
                .Map(
                    user => user.Email,
                    updateUserCommand => updateUserCommand.NewEmail);
        }

        private IServiceCollection AddDatabase(
            AppOptions appOptions,
            IHostEnvironment environment)
        {
            if (appOptions.DatabaseProvider == StringConstants.SqlServer)
            {
                return services.AddSqlServer(appOptions, environment);
            }

            if (appOptions.DatabaseProvider == StringConstants.Sqlite)
            {
                return services.AddSqlite(appOptions, environment);
            }

            var message = string.Format(
                StringConstants.InvalidDatabaseProviderException,
                appOptions.DatabaseProvider,
                StringConstants.SqlServer,
                StringConstants.Sqlite);

            throw new InvalidDataException(message);
        }

        private IServiceCollection AddSqlServer(
            AppOptions appOptions,
            IHostEnvironment environment)
        {
            if (environment.IsEnvironment(InfrastructureConstants.TestingEnvironmentName))
            {
                return services.AddDbContext<IUnitOfWork, UnitOfWork>(options =>
                {
                    options
                        .UseInMemoryDatabase(appOptions.ApplicationInformation!.Name!)
                        .UseSeeding((dbContext, _) => dbContext.SeedDatabase())
                        .UseAsyncSeeding((dbContext, _, _) => Task.FromResult(dbContext.SeedDatabase()));
                });
            }

            return services.AddDbContext<IUnitOfWork, UnitOfWork>(options =>
            {
                options
                    .UseSqlServer(appOptions.SqlServerConnectionString)
                    .UseSeeding((dbContext, _) => dbContext.SeedDatabase())
                    .UseAsyncSeeding((dbContext, _, _) => Task.FromResult(dbContext.SeedDatabase()));

                if (!environment.IsProduction())
                {
                    options.EnableSensitiveDataLogging();
                }
            });
        }

        private IServiceCollection AddSqlite(
            AppOptions appOptions,
            IHostEnvironment environment) =>
            services.AddDbContext<IUnitOfWork, SqliteUnitOfWork>(options =>
            {
                options
                    .UseSqlite(appOptions.SqliteConnectionString)
                    .UseSeeding((dbContext, _) => dbContext.SeedDatabase())
                    .UseAsyncSeeding((dbContext, _, _) => Task.FromResult(dbContext.SeedDatabase()));

                if (!environment.IsProduction())
                {
                    options.EnableSensitiveDataLogging();
                }
            });
    }

    extension(DbContext dbContext)
    {
        private DbContext SeedDatabase()
        {
            var rolesDbSet = dbContext.Set<Role>();

            if (!rolesDbSet.Any())
            {
                rolesDbSet.AddRange(
                    new Role
                    {
                        Title = DomainConstants.AdminRoleTitle,
                        Description = "Administrator of the Application"
                    },
                    new Role
                    {
                        Title = DomainConstants.BasicUserRoleTitle,
                        Description = "Basic User of the Application"
                    });

                dbContext.SaveChanges();
            }

            if (dbContext.Set<User>().Any())
            {
                return dbContext;
            }

            const int fakeUsersCount = 1_000;
            const int fakeBlogPostsCount = 1_000;

            var fakeUsers = FakeDataHelper.GetFakeUsers(fakeUsersCount);

            dbContext.Set<User>().AddRange(fakeUsers);

            dbContext.SaveChanges();

            var fakeBlogPosts = FakeDataHelper.GetFakeBlogPosts(fakeUsers, fakeBlogPostsCount);

            dbContext.Set<BlogPost>().AddRange(fakeBlogPosts);

            dbContext.SaveChanges();

            var fakeUserRoles = FakeDataHelper.GetFakeUserRoles(fakeUsers, dbContext.Set<Role>().ToList());

            dbContext.Set<UserRole>().AddRange(fakeUserRoles);

            dbContext.SaveChanges();

            return dbContext;
        }
    }
}