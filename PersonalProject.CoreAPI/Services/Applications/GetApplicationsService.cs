using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.Provider.Providers.Applications;

namespace PersonalProject.CoreAPI.Services.Applications;

public interface IGetApplicationsService
{

    Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber);
    Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView();

    Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter);

    Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync();
}
public class GetApplicationsService : IGetApplicationsService
{
    private readonly IGetApplicationsProvider _getApplicationsProvider;

    public GetApplicationsService(IGetApplicationsProvider applicationsProvider)
    {
        _getApplicationsProvider = applicationsProvider;
    }

    public async Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView()
    {
        return await _getApplicationsProvider.GetAllApplicationsDashboardView();
    }

    public async Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync()
        => await _getApplicationsProvider.GetAllApplicationStatusesAsync();

    public async Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber)
     => await _getApplicationsProvider.GetApplicationByReferenceNumberAsync(refNumber);

    public Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter)
    {
        throw new NotImplementedException();
    }
}
