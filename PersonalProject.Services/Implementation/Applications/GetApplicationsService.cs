using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.Provider.Providers.Interfaces.ApplicationsProvider;
using PersonalProject.Services.Interfaces.Applications;

namespace PersonalProject.Services.Implementation.Applications;
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

    public async Task<IEnumerable<ApplicationStatus>> GetAllApplicationsStatusAsync()
        => await _getApplicationsProvider.GetAllApplicationsStatusesAsync();

    public async Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber)
     => await _getApplicationsProvider.GetApplicationByReferenceNumberAsync(refNumber);

    public Task<PagedResult<ApplicationDashboard>> GetPagedApplications(int page = 1, int pageSize = 20, string sortBy = "RefNumber", bool orderByDescending = true, List<string>? filterApplicationStatusBy = null, string? isBeingAudited = null, string? searchBy = null)
    {
        throw new NotImplementedException();
    }
}
