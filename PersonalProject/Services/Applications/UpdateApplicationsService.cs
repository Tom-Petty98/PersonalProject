using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.InternalPortal.Services.Interfaces;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Implementation;

public interface IUpdateApplicationsService
{
    Task<Application> AddApplication(Application application);

    Task<bool> UpdateApplication(Application application);

    Task<bool> UpdateApplicationDetail(ApplicationDetail applicationDetail);
}

public class ApplicationsService : IUpdateApplicationsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPolicyRegistry<string> _polySettings;
    private const string _clientName = "CoreAPI";
    public ApplicationsService(IHttpClientFactory httpClientFactory, IPolicyRegistry<string> polySettings)
    {
        _httpClientFactory = httpClientFactory;
        _polySettings = polySettings;
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
