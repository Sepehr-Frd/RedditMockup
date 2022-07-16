using System.IO.Compression;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using RedditMockup.Api.Contracts;
using RedditMockup.Api.Filters;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Contracts;
using RedditMockup.Common.Profiles;
using RedditMockup.Common.Validations;
using RedditMockup.DataAccess;
using RedditMockup.DataAccess.Context;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.Web;

internal static class DependencyInjectionExtension
{
    internal static IServiceCollection InjectApi(this IServiceCollection services) =>
        services
            .AddControllers()
            //.AddControllers(x => x.Filters.Add<CustomExceptionFilter>())
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            })
            .AddApplicationPart(typeof(IBaseController<BaseEntity, IBaseDto>).Assembly)
            .Services
            .AddHealthChecks()
            .Services;

    internal static IServiceCollection InjectSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen();

    internal static IServiceCollection InjectUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    internal static IServiceCollection InjectContext(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment) =>
        environment.IsDevelopment() || environment.IsEnvironment("Testing")
            ? services.AddDbContextPool<RedditMockupContext>(options =>
                options.UseInMemoryDatabase("RedditMockup"))
            : services.AddDbContextPool<RedditMockupContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));
    internal static IServiceCollection InjectNLog(this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        var factory = NLogBuilder.ConfigureNLog(
                environment.IsProduction()
                    ? "nlog.config"
                    : $"nlog.{environment.EnvironmentName}.config");
        return services.AddSingleton(_ => factory.GetLogger("Info"))
            .AddSingleton(_ => factory.GetLogger("Error"));
    }

    internal static IServiceCollection InjectSieve(this IServiceCollection services) =>
        services.AddScoped<ISieveProcessor, SieveProcessor>();

    internal static IServiceCollection InjectAuthentication(this IServiceCollection services) =>
        services
            .AddAuthorization()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = options.DefaultChallengeScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.AccessDeniedPath = "/Account/AccessDenied";
            })
            .Services;

    internal static IServiceCollection InjectBusinesses(this IServiceCollection services) =>
        services.Scan(scan =>
                scan.FromAssembliesOf(typeof(IBaseBusiness<>))
                    .AddClasses(classes =>
                        classes.AssignableTo(typeof(IBaseBusiness<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(classes =>
                        classes.Where(predicate =>
                            predicate.Name.EndsWith("Business") && !predicate.IsAssignableTo(typeof(IBaseBusiness<>))))
                    .AsSelf()
                    .WithScopedLifetime());

    internal static IServiceCollection InjectContentCompression(this IServiceCollection services) =>
        services.Configure<GzipCompressionProviderOptions>
                (options => options.Level = CompressionLevel.Fastest)
            .AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());

    internal static IServiceCollection InjectFluentValidation(this IServiceCollection services) =>
        services.AddFluentValidation(fv =>
            fv.RegisterValidatorsFromAssemblyContaining<RoleValidator>());

    internal static IServiceCollection InjectAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(UserProfile).Assembly);
}