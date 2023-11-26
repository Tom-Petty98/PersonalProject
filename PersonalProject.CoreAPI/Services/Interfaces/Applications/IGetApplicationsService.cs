using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;

namespace PersonalProject.CoreAPI.Services.Interfaces.Applications;
public interface IGetApplicationsService
{

    Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber);
    Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView();

    Task<PagedResult<ApplicationDashboard>> GetPagedApplications(int page = 1, int pageSize = 20,
        string sortBy = "RefNumber", bool orderByDescending = true,
        List<string>? filterApplicationStatusBy = null,
        string? isBeingAudited = null, string? searchBy = null);

    Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync();
}