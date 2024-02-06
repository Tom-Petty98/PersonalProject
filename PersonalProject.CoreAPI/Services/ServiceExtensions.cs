using PersonalProject.CoreAPI.Services.Applications;
using PersonalProject.CoreAPI.Services.Installers;

namespace PersonalProject.CoreAPI.Services;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IGetApplicationsService, GetApplicationsService>();
        services.AddTransient<IUpdateApplicationService, UpdateApplicationService>();
        services.AddTransient<IGetInstallersService, GetInstallersService>();
        services.AddTransient<IUpdateInstallerService, UpdateInstallerService>();
        services.AddTransient<IGetUsersService, GetUsersService>();
        services.AddTransient <IUpdateUsersService, UpdateUsersService>();
        return services;
    }
}