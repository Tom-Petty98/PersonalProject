using DocumentsAPI;
using DocumentsAPI.Providers;
using DocumentsAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Tests.DocumentsAPI.IntergrationTests.Mocks;

namespace Tests.DocumentsAPI.IntergrationTests;

public class DocumentsWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntergrationTest");
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.Remove(services.SingleOrDefault(x => x.ServiceType == typeof(IAzureBlobStorageService))!);
            services.AddSingleton(typeof(IAzureBlobStorageService), new AzureBlobStorageServiceMock());

            services.Remove(services.SingleOrDefault(x => x.ServiceType == typeof(DocumentsDbContext))!);
            services.AddSingleton(typeof(DocumentsDbContext), new DocumentsDbContextFactory().CreateDbContext(Array.Empty<string>()));
        });
    }
}
