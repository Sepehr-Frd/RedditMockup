namespace InsightFlow.Common.Constants;

public static class ApplicationConstants
{
    // ---------------------------- File Extensions Constants ----------------------------
    public static readonly string[] ValidProfileImageFormats = [Png, Jpeg, Jpg];
    private const string Png = "png";
    public const string Jpeg = "jpeg";
    public const string Jpg = "jpg";

    // ---------------------------- Auth Constants ----------------------------
    public const string AdminPolicyName = "OnlyAdminPolicy";
    public const string UserPolicyName = "OnlyUserPolicy";
    public const string AdminRoleName = "Admin";
    public const string UserRoleName = "User";
    public const string UsernameClaim = "username";
    public const string ExternalIdClaim = "external_id";

    // ---------------------------- CORS Constants ----------------------------
    public const string AllowAnyOriginCorsPolicy = "AllowAnyOriginCorsPolicy";
    public const string RestrictedCorsPolicy = "RestrictedCorsPolicy";

    // ---------------------------- Configuration Key Constants ----------------------------
    public const string ApplicationVersionConfigurationKey = "ApplicationVersion";
    public const string ApplicationUrlsConfigurationSectionKey = "ApplicationUrls";
    public const string SqlServerConfigurationKey = "SqlServer";
    public const string ServerUrlConfigurationKey = "ServerUrl";
    public const string ClientUrlConfigurationKey = "ClientUrl";
    public const string JwtConfigurationSectionKey = "JwtConfiguration";
    public const string JwtPublicKeyConfigurationKey = "PublicKey";
    public const string JwtPrivateKeyConfigurationKey = "PrivateKey";
    public const string CaptchaConfigurationSectionKey = "CaptchaConfiguration";
    public const string RateLimitersSectionKey = "RateLimitersConfiguration";

    public const string DntCaptchaName = "dntcaptcha";
    
    public const string FixedWindowRateLimiterPolicy = "FixedWindowRateLimiterPolicy";
    public const string ConcurrencyRateLimiterPolicy = "ConcurrencyRateLimiterPolicy";

    public const string TestingEnvironmentName = "Testing";

    public const string ApplicationName = "InsightFlow";
}