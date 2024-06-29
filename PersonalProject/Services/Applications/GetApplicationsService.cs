using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.InternalPortal.Services.Helpers;
using PersonalProject.InternalPortal.Services.Installers;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Applications;

public interface IGetApplicationsService
{
    Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber);
    Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync();
    Task<IEnumerable<TechType>> GetTechTypesAsync();
    Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView();
    Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter);
}

public class GetApplicationsService : BaseRequestsClient<GetApplicationsService>, IGetApplicationsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "CoreAPI";
    public GetApplicationsService(IHttpClientFactory httpClientFactory, ILogger<GetApplicationsService> logger, IPolicyRegistry<string> polySettings)
        : base(polySettings, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public async Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync()
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetAllApplicationStatusesAsync));
        var target = "Applications/GetAllApplicationStatuses";

        var responseObject = await GetAsync<IEnumerable<ApplicationStatus>>(httpClient, target, null, null);
        return responseObject ?? new List<ApplicationStatus>();
    }

    public async Task<IEnumerable<TechType>> GetTechTypesAsync()
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetTechTypesAsync));
        var target = "Applications/GetTechTypes";

        var responseObject = await GetAsync<IEnumerable<TechType>>(httpClient, target, null, null);
        return responseObject ?? new List<TechType>();
    }

    public async Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetApplicationByReferenceNumberAsync));
        var target = $"Applications/GetApplicationByReferenceNumber/{refNumber}";

        return await GetAsync<Application?>(httpClient, target, null, null);
    }

    public async Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView()
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetAllApplicationsDashboardView));
        var target = "Applications/GetAllApplicationsDashboardView";

        var responseObject = await GetAsync<List<ApplicationDashboard>>(httpClient, target, null, null);
        return responseObject ?? new List<ApplicationDashboard>();
    }

    public async Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetAllApplicationsDashboardView));
        var target = "Applications/GetPagedApplications";

        var responseObject = await GetAsync<DashboardFilter, PagedResult<ApplicationDashboard>>(httpClient, target, dashboardFilter, null, null);
        return responseObject ?? new PagedResult<ApplicationDashboard>();
    }
}
