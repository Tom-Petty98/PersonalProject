using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.InternalPortal.Services.Helpers;
using PersonalProject.InternalPortal.Services.Interfaces;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Installers;

public class InstallerService : BaseRequestsClient<InstallerService>, IInstallerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "CoreAPI";
    public InstallerService(IHttpClientFactory httpClientFactory, ILogger<InstallerService> logger, IPolicyRegistry<string> polySettings)
        : base (polySettings, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public async Task<Installer> AddInstaller(Installer installer)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(AddInstaller));
        var target = "Installers/AddInstaller";

        return await PostAsync<Installer, Installer>(httpClient, target, installer, null, null)
            ?? throw new Exception("Null response recieved");
    }

    public async Task<IEnumerable<InstallerDashboard>> GetAllInstallersDashboardView()
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetAllInstallersDashboardView));
        var target = "Installers/GetAllInstallersDashboardView";

        var responseObject = await GetAsync<IEnumerable<InstallerDashboard>>(httpClient, target, null, null);
        return responseObject ?? new List<InstallerDashboard>();
    }

    public Task<IEnumerable<InstallerStatus>> GetAllInstallerStatusesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<InstallerDashboard>> GetPagedInstallers(DashboardFilter dashboardFilter)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateInstaller(Installer installer)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail)
    {
        throw new NotImplementedException();
    }
}
