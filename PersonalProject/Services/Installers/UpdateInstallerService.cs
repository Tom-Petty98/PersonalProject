using PersonalProject.Domain.Entities;
using PersonalProject.InternalPortal.Services.Helpers;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Installers;

public interface IUpdateInstallerService
{
    Task<Installer> AddInstaller(Installer installer);
    Task<Installer> UpdateInstaller(Installer installer);
    Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail);
}

public class UpdateInstallerService : BaseRequestsClient<GetInstallerService>, IUpdateInstallerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "CoreAPI";
    public UpdateInstallerService(IHttpClientFactory httpClientFactory, ILogger<GetInstallerService> logger, IPolicyRegistry<string> polySettings)
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

    public async Task<Installer> UpdateInstaller(Installer installer)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(UpdateInstaller));
        var target = "Installers/UpdateInstaller";

        return await PostAsync<Installer, Installer>(httpClient, target, installer, null, null)
            ?? throw new Exception("Null response recieved");
    }

    public Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail)
    {
        throw new NotImplementedException();
    }
}
