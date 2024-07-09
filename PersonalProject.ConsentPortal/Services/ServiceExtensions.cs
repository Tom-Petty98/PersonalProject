using PersonalProject.ConsentPortal.Services;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;
using Polly;

namespace PersonalProject.ConsentPortal.Services;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient("CoreAPI", c =>
        {
            c.BaseAddress = new Uri(config["applicationUrl"]!);
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        string sessionTokenSecret = config["SessionTokenSecret"]!;

        services.AddPollyPolicies(config);

        services.AddTransient<IConsentService, ConsentService>();
        services.AddTransient<ISessionAuthorizationService>(service => new SessionAuthorizationService(sessionTokenSecret));

        services.AddTransient<SessionTokenAuthorizeAttribute>();

        return services;
    }

    private static void AddPollyPolicies(this IServiceCollection services, IConfiguration config)
    {
        var pollyConfig = config.GetSection("PollySettings");
        var policiesToAdd = new Dictionary<string, IAsyncPolicy<HttpResponseMessage>>
        {
            {PollyContextKeys.RetryHttp500, PollyExtensions.Http500RetryPolicy(pollyConfig)}
        };

        services.AddPollyPolicies(policiesToAdd);
    }
}