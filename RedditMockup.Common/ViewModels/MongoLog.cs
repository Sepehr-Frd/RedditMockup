namespace RedditMockup.Common.ViewModels;

public class MongoLog
{
    public dynamic? Request { get; set; }

    public dynamic? Response { get; set; }

    public TimeSpan ElapsedTime { get; set; }

    public string? ClientIp { get; set; }

    public string? ServerIp { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public string? Username { get; set; }

    public Exception? Exception { get; set; }
}