using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.InternalPortal.Services.Helpers;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Installers;

public interface IGetInstallerService
{
    Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber);
    Task<Installer?> GetInstallerByIdAsync(int installerId);
    Task<string> GetInstallerNameByIdAsync(int installerId);
    Task<IEnumerable<InstallerStatus>> GetAllInstallerStatusesAsync();
    Task<IEnumerable<InstallerDashboard>> GetAllInstallersDashboardView();
    Task<PagedResult<InstallerDashboard>> GetPagedInstallers(DashboardFilter dashboardFilter);
}

public class GetInstallerService : BaseRequestsClient<GetInstallerService>, IGetInstallerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "CoreAPI";
    public GetInstallerService(IHttpClientFactory httpClientFactory, ILogger<GetInstallerService> logger, IPolicyRegistry<string> polySettings)
        : base (polySettings, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public async Task<IEnumerable<InstallerDashboard>> GetAllInstallersDashboardView()
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetAllInstallersDashboardView));
        var target = "Installers/GetAllInstallersDashboardView";

        var responseObject = await GetAsync<List<InstallerDashboard>>(httpClient, target, null, null);
        return responseObject ?? new List<InstallerDashboard>();
    }

    public async Task<IEnumerable<InstallerStatus>> GetAllInstallerStatusesAsync()
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetAllInstallerStatusesAsync));
        var target = "Installers/GetAllInstallerStatuses";

        var responseObject = await GetAsync<IEnumerable<InstallerStatus>>(httpClient, target, null, null);
        return responseObject ?? new List<InstallerStatus>();
    }

    public async Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetInstallerByReferenceNumberAsync));
        var target = $"Installers/GetInstallerByReferenceNumber/{refNumber}";

        return await GetAsync<Installer?>(httpClient, target, null, null);
    }

    public async Task<Installer?> GetInstallerByIdAsync(int installerId)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetInstallerByIdAsync));
        var target = $"Installers/GetInstallerById/{installerId}";

        return await GetAsync<Installer?>(httpClient, target, null, null);
    }

    public async Task<string> GetInstallerNameByIdAsync(int installerId)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetInstallerNameByIdAsync));
        var target = $"Installers/GetInstallerNameById/{installerId}";

        return await GetAsync<string>(httpClient, target, null, null) ?? "";
    }

    public async Task<PagedResult<InstallerDashboard>> GetPagedInstallers(DashboardFilter dashboardFilter)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetPagedInstallers));
        var target = "Installers/GetAllInstallersDashboardView";

        var responseObject = await GetAsync<PagedResult<InstallerDashboard>>(httpClient, target, null, null);
        return responseObject ?? new PagedResult<InstallerDashboard>();
    }   
}
