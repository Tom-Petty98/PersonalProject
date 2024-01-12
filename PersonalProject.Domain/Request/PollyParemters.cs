namespace PersonalProject.Domain.Request;

public class PollyParemters
{
    /// <summary>
    /// The policy key for a policy already registered
    /// </summary>
    public string PolicyKey { get; set; } = string.Empty;
    /// <summary>
    /// The name of the orginating method or endpoint
    /// </summary>
    public string Source { get; set; } = string.Empty;
    /// <summary>
    /// Optinal key value pairs to add to the Polly context.
    /// Keys for the RetryCount, Source, Logger are added by default.
    /// <see cref="PollyContextKeys"/>
    /// </summary>
    public IDictionary<string, object>? ContextParameters { get; set; }
}

public static class PollyContextKeys
{
    public const string RetryCount = "retryCount";
    public const string Source = "source";
    public const string Logger = "logger";
    public const string RetryHttp500 = "RetryHttp500";
}

public static class PollyPolicySettings
{
    public const string Http500RetryCount = "Http500RetryCount";
    public const string Http500RetryInterval = "Http500RetryInterval";
}
