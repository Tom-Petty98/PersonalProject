using PersonalProject.Domain.Entities;
using PersonalProject.InternalPortal.Services.Helpers;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Implementation;

public interface IUpdateApplicationsService
{
    Task<Application> AddApplication(Application application);

    Task<bool> UpdateApplication(Application application);

    Task<bool> UpdateApplicationDetail(ApplicationDetail applicationDetail);
}

public class UpdateApplicationsService : BaseRequestsClient<UpdateApplicationsService>, IUpdateApplicationsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "CoreAPI";
    public UpdateApplicationsService(IHttpClientFactory httpClientFactory, ILogger<UpdateApplicationsService> logger, IPolicyRegistry<string> polySettings)
        : base(polySettings, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public async Task<Application> AddApplication(Application application)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(AddApplication));
        var target = "Applications/AddApplication";

        return await PostAsync<Application, Application>(httpClient, target, application, null, null)
            ?? throw new Exception("Null response recieved");
    }

    public async Task<bool> UpdateApplication(Application application)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(UpdateApplication));
        var target = "Applications/UpdateApplication";

        return await PostAsync<Application, bool>(httpClient, target, application, null, null);
    }

    public async Task<bool> UpdateApplicationDetail(ApplicationDetail applicationDetail)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(UpdateApplicationDetail));
        var target = "Applications/UpdateApplicationDetail";

        return await PostAsync<ApplicationDetail, bool>(httpClient, target, applicationDetail, null, null);
    }
}
