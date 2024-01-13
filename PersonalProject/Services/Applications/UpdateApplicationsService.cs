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

    public Task<Application> AddApplication(Application application)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateApplication(Application application)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateApplicationDetail(ApplicationDetail applicationDetail)
    {
        throw new NotImplementedException();
    }
}
