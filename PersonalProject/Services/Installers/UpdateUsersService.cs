using PersonalProject.Domain.Entities;
using PersonalProject.InternalPortal.Services.Helpers;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Installers;

public interface IUpdateUsersService
{
    Task<int> AddUser(User user);
    Task<bool> UpdateUser(User user);
}

public class UpdateUsersService : BaseRequestsClient<UpdateUsersService>, IUpdateUsersService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string _clientName = "CoreAPI";
    public UpdateUsersService(IHttpClientFactory httpClientFactory, ILogger<UpdateUsersService> logger, IPolicyRegistry<string> polySettings)
        : base(polySettings, logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public async Task<int> AddUser(User user)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(AddUser));
        var target = "Users/AddUser";

        var userId = await PostAsync<User, int>(httpClient, target, user, null, null);
        return userId;
    }

    public async Task<bool> UpdateUser(User user)
    {
        var httpClient = BuildClient();
        var pollyParams = PollyExtensions.BuildPollyParams(nameof(UpdateUser));
        var target = "Users/UpdateUser";

        return await PostAsync<User, bool>(httpClient, target, user, null, null);
    }
}