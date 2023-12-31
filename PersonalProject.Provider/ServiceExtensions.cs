﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalProject.Provider.Data;
using PersonalProject.Provider.Providers.Implementation.Applications;
using PersonalProject.Provider.Providers.Implementation.Installers;
using PersonalProject.Provider.Providers.Interfaces.Applications;
using PersonalProject.Provider.Providers.Interfaces.Installers;

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

        return services;
    }
}