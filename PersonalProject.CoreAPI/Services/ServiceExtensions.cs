using PersonalProject.CoreAPI.Services.Applications;
using PersonalProject.CoreAPI.Services.Installers;
using PersonalProject.CoreAPI.Services.Shared;

namespace PersonalProject.CoreAPI.Services;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        //services.AddAutoMapperConfiguration();

        Config apiConfig = config.GetRequiredSection("Api").Get<Config>()!;

        services.AddSingleton(apiConfig);
        services.AddTransient<IConsentService, ConsentService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IJwtTokenService, JwtTokenService>();

        services.AddTransient<IGetApplicationsService, GetApplicationsService>();
        services.AddTransient<IUpdateApplicationService, UpdateApplicationService>();
        services.AddTransient<IGetInstallersService, GetInstallersService>();
        services.AddTransient<IUpdateInstallerService, UpdateInstallerService>();
        services.AddTransient<IGetUsersService, GetUsersService>();
        services.AddTransient <IUpdateUsersService, UpdateUsersService>();

        return services;
    }
}