using PersonalProject.Domain.Request;
using Polly;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Helpers;

public static class PollyExtensions
{
    public static Context BuildPollyContext(this PollyParemters paremters, ILogger logger)
    {
        var context = new Context()
        {
            { PollyContextKeys.RetryCount, 0 },
            { PollyContextKeys.Source, paremters.Source },
            { PollyContextKeys.Logger, logger }
        };

        if (paremters.ContextParameters != null && paremters.ContextParameters.Any())
        {
            foreach (var kvp in paremters.ContextParameters)
            {
                if (context.ContainsKey(kvp.Key))
                    context.Remove(kvp.Key);

                context.Add(kvp.Key, kvp.Value);
            }
        }

        return context;
    }

    public static PollyParemters BuildPollyParams(string source, string? policyKey = null)
    {
        return new PollyParemters
        {
            PolicyKey = policyKey ?? PollyContextKeys.RetryHttp500,
            Source = source
        };
    }

    public static void AddPollyPolicies<T>(this IServiceCollection services, IDictionary<string, IAsyncPolicy<T>> policies)
    {
        var builder = services.BuildServiceProvider();
        var policyRegistry = builder.GetService<IPolicyRegistry<string>>() ?? new PolicyRegistry();

        foreach(var policy in policies)
        {
            if (!policyRegistry.ContainsKey(policy.Key))
                policyRegistry.Add(policy.Key, policy.Value);
        }
        services.AddPolicyRegistry(policyRegistry);
    }

    public static IAsyncPolicy<HttpResponseMessage> Http500RetryPolicy(IConfiguration pollySettings)
    {
        return Policy.HandleResult<HttpResponseMessage>(x => x.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            .WaitAndRetryAsync(int.Parse(pollySettings[PollyPolicySettings.Http500RetryCount]),
                retryCount => TimeSpan.FromMilliseconds(int.Parse(pollySettings[PollyPolicySettings.Http500RetryInterval])),
                (result, timeSpan, retryCount, context) =>
                {
                    if (!context.TryGetLogger(out var logger)) return;

                    if (result != null)
                    {
                        logger.LogError(result.Exception, $"An exception occurred on retry {retryCount} for {context[PollyContextKeys.Source]}");
                    }
                    else
                    {
                        logger.LogError($"A non success coid was recieved on retry {retryCount} " +
                            $"for {context[PollyContextKeys.Source]}. Exception: {result} ");
                    }
                });
    }

    public static bool TryGetLogger(this Context context, out ILogger logger)
    {
        if (context.TryGetValue("Logger", out var loggerObject) && loggerObject is ILogger theLogger)
        {
            logger = theLogger;
            return true;
        }

        logger = null!;
        return false;
    }
}
