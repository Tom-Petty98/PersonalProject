using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using Polly.Registry;

namespace PersonalProject.InternalPortal.Services.Applications;

public interface IGetApplicationsService
{
    Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber);
    Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView();

    Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync();

    Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter);
}

public class GetApplicationsService : IGetApplicationsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPolicyRegistry<string> _polySettings;
    private const string _clientName = "CoreAPI";
    public GetApplicationsService(IHttpClientFactory httpClientFactory, IPolicyRegistry<string> polySettings)
    {
        _httpClientFactory = httpClientFactory;
        _polySettings = polySettings;
    }

    private HttpClient BuildClient() => _httpClientFactory.CreateClient(_clientName);

    public Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter)
    {
        throw new NotImplementedException();
    }
}
