using PersonalProject.InternalPortal.Services.Applications;
using PersonalProject.InternalPortal.Services.Installers;
using PersonalProject.InternalPortal.Services.Interfaces;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace PersonalProject.InternalPortal.Services;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        var retryPolicy = HttpPolicyExtensions
          .HandleTransientHttpError()
          .Or<TimeoutRejectedException>() // thrown by Polly's TimeoutPolicy if the inner execution times out
          .RetryAsync(3);

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);

        services.AddHttpClient("CoreAPI", c =>
            {
                c.BaseAddress = new Uri(config["applicationUrl"]!);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        services.AddTransient<IApplicationsService, GetApplicationsService>();
        services.AddTransient<IInstallerService, InstallerService>();

        return services;
    }
}