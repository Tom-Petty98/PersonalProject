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
    Task<IEnumerable<TechType>> GetTechTypesAsync();
}

public class GetApplicationsService : IGetApplicationsService
{
    private readonly IGetApplicationsProvider _getApplicationsProvider;
    private readonly IGetAppDashboardProvider _getAppDashboardProvider;

    public GetApplicationsService(IGetApplicationsProvider applicationsProvider, IGetAppDashboardProvider getAppDashboardProvider)
    {
        _getApplicationsProvider = applicationsProvider;
        _getAppDashboardProvider = getAppDashboardProvider;
    }

    public async Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView()
    {
        return await _getApplicationsProvider.GetAllApplicationsDashboardView();
    }

    public async Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync()
        => await _getApplicationsProvider.GetAllApplicationStatusesAsync();

    public async Task<IEnumerable<TechType>> GetTechTypesAsync()
        => await _getApplicationsProvider.GetTechTypesAsync();

    public async Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber)
     => await _getApplicationsProvider.GetApplicationByReferenceNumberAsync(refNumber);

    public async Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter)
        => await _getAppDashboardProvider.GetPagedApplications(dashboardFilter);
    
}
