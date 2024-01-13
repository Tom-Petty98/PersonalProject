using PersonalProject.Domain.Request;
using PersonalProject.InternalPortal.Services.Applications;
using PersonalProject.InternalPortal.Services.Helpers;
using PersonalProject.InternalPortal.Services.Implementation;
using PersonalProject.InternalPortal.Services.Installers;
using Polly;

namespace PersonalProject.InternalPortal.Services;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient("CoreAPI", c =>
        {
            c.BaseAddress = new Uri(config["applicationUrl"]!);
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddPollyPolicies(config);

        services.AddTransient<IGetApplicationsService, GetApplicationsService>();
        services.AddTransient<IUpdateApplicationsService, UpdateApplicationsService>();
        services.AddTransient<IGetInstallerService, GetInstallerService>();
        services.AddTransient<IUpdateInstallerService, UpdateInstallerService>();

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