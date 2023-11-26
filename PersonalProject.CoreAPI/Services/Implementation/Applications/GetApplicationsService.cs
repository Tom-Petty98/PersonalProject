using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.Provider.Providers.Interfaces.Applications;
using PersonalProject.CoreAPI.Services.Interfaces.Applications;

namespace PersonalProject.CoreAPI.Services.Implementation.Applications;
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

    public Task<PagedResult<ApplicationDashboard>> GetPagedApplications(int page = 1, int pageSize = 20, string sortBy = "RefNumber", bool orderByDescending = true, List<string>? filterApplicationStatusBy = null, string? isBeingAudited = null, string? searchBy = null)
    {
        throw new NotImplementedException();
    }
}
