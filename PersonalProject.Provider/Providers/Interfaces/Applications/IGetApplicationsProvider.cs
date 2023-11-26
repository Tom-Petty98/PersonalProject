using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;

namespace PersonalProject.Provider.Providers.Interfaces.Applications;

public interface IGetApplicationsProvider
{
    Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber);
    Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView();

    Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync();

    Task<PagedResult<ApplicationDashboard>> GetPagedApplications(int page = 1, int pageSize = 20,
        string sortBy = "RefNumber", bool orderByDescending = true,
        List<string>? filterApplicationStatusBy = null,
        string? isBeingAudited = null, string? searchBy = null);
}
