using PersonalProject.Domain.Entities;
using PersonalProject.InternalPortal.Services.Helpers;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Installers;

public interface IGetUsersService
{
    Task<List<User>?> GetUsersByInstallerIdAsync(int userId);
    Task<User?> GetUserByIdAsync(int userId);
    Task<IEnumerable<Role>> GetUserRolesAsync(bool getInternal);
}

public class GetUsersService : BaseRequestsClient<GetUsersService>, IGetUsersService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "CoreAPI";
    public GetUsersService(IHttpClientFactory httpClientFactory, ILogger<GetUsersService> logger, IPolicyRegistry<string> polySettings)
        : base(polySettings, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public async Task<List<User>?> GetUsersByInstallerIdAsync(int installerId)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetUsersByInstallerIdAsync));
        var target = $"Users/GetUsersByInstallerId/{installerId}";

        var responseObject = await GetAsync<List<User>>(httpClient, target, null, null);
        return responseObject ?? new List<User>();
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(bool getInternal)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetUserRolesAsync));
        var target = $"Users/GetUserRoles/{getInternal}";

        var responseObject = await GetAsync<IEnumerable<Role>>(httpClient, target, null, null);
        return responseObject ?? new List<Role>();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(GetUserByIdAsync));
        var target = $"Users/GetUserById/{userId}";

        return await GetAsync<User?>(httpClient, target, null, null);
    }
}
