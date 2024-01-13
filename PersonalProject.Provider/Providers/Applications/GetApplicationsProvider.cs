using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Provider.Providers.Applications;

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

public class GetApplicationsProvider : IGetApplicationsProvider
{
    private readonly ApplicationDbContext _context;
    public GetApplicationsProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApplicationDashboard>> GetAllApplicationsDashboardView()
    {
        return await _context.ApplicationDashboards.OrderByDescending(x => x.RefNumber).ToListAsync();
    }

    public async Task<IEnumerable<ApplicationStatus>> GetAllApplicationStatusesAsync()
    {
        return await _context.ApplicationStatuses.OrderBy(x => x.SortOrder).ToListAsync();
    }

    public async Task<Application?> GetApplicationByReferenceNumberAsync(string refNumber)
    {
        return await _context.Applications
            .Include(x => x.Status)
            .Include(x => x.ApplicationDetail)
            .Include(x => x.ApplicationDetail.InstallationAddress)
            .FirstOrDefaultAsync(x => x.RefNumber == refNumber);
    }

    public Task<PagedResult<ApplicationDashboard>> GetPagedApplications(int page = 1, int pageSize = 20,
        string sortBy = "RefNumber", bool orderByDescending = true, List<string>? filterApplicationStatusBy = null,
        string? isBeingAudited = null, string? searchBy = null)
    {
        throw new NotImplementedException();
    }


}

