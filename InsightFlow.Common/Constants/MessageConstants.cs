namespace InsightFlow.Common.Constants;

public static class MessageConstants
{
    // ---------------------------- Bad Request (400) Messages ----------------------------
    public const string AlreadySignedInMessage = "You are already signed in.";
    public const string InvalidCredentialsMessage = "Username and/or password not correct.";
    public const string InvalidParametersMessage = "Invalid value for parameter(s) {0}.";

    // ---------------------------- Not Found (404) Messages ----------------------------
    public const string EntityNotFoundMessage = "{0} with guid of {1} not found.";

    // ---------------------------- OK (200) Messages ----------------------------
    public const string SuccessfulLoginMessage = "Successfully logged in.";

    // ---------------------------- Exception Messages ----------------------------
    public const string ExceptionMessage = "Application encountered an unhandled exception of type: {exceptionType}";
    public const string ApplicationFatalExceptionMessage = "Application stopped due to a {exceptionType} exception";

    public const string SwaggerAuthorizationMessage = "Please enter only the token (without Bearer)";
}