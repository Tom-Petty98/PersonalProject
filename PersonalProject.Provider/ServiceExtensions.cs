using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalProject.Provider.Providers.Applications;
using PersonalProject.Provider.Providers.Installers;
using PersonalProject.Provider.Providers.Shared;

namespace PersonalProject.Provider;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureDataProvider(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("PersonalProjectDB"),
                provider =>
                {
                    provider.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    provider.EnableRetryOnFailure();
                });
        });

        services.AddTransient<IGetApplicationsProvider, GetApplicationsProvider>();
        services.AddTransient<IUpdateApplicationsProvider, UpdateApplicationsProvider>();
        services.AddTransient<IGetInstallersProvider, GetInstallersProvider>();
        services.AddTransient<IUpdateInstallersProvider, UpdateInstallersProvider>();
        services.AddTransient<IGetUsersProvider, GetUsersProvider>();
        services.AddTransient<IUpdateUsersProvider, UpdateUsersProvider>();
        services.AddTransient<IAuditLogsProvider, AuditLogsProvider>();

        return services;
    }
}