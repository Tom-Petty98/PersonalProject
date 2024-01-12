using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.Provider.Data;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Provider.Providers.Installers;

public interface IGetInstallersProvider
{
    Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber);
    Task<IEnumerable<InstallerDashboard>> GetAllInstallersDashboardView();

    Task<IEnumerable<InstallerStatus>> GetAllInstallerStatusesAsync();

    Task<PagedResult<InstallerDashboard>> GetPagedInstallers(int page = 1, int pageSize = 20,
        string sortBy = "RefNumber", bool orderByDescending = true,
        List<string>? filterInstallerStatusBy = null,
        string? isBeingAudited = null, string? searchBy = null);
}

public class GetInstallersProvider : IGetInstallersProvider
{
    private readonly ApplicationDbContext _context;
    public GetInstallersProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InstallerDashboard>> GetAllInstallersDashboardView()
    {
        return await _context.InstallerDashboards.OrderByDescending(x => x.RefNumber).ToListAsync();
    }

    public async Task<IEnumerable<InstallerStatus>> GetAllInstallerStatusesAsync()
    {
        return await _context.InstallerStatuses.OrderBy(x => x.SortOrder).ToListAsync();
    }

    public async Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber)
    {
        return await _context.Installers
            .Include(x => x.Status)
            .Include(x => x.InstallerDetail)
            .Include(x => x.InstallerDetail.InstallerAddress)
            .FirstOrDefaultAsync(x => x.RefNumber == refNumber);
    }

    public Task<InstallerDetail?> GetInstallerDetailByReferenceNumberAsync(string refNumber)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<InstallerDashboard>> GetPagedInstallers(int page = 1, int pageSize = 20,
        string sortBy = "RefNumber", bool orderByDescending = true, List<string>? filterInstallerStatusBy = null,
        string? isBeingAudited = null, string? searchBy = null)
    {
        throw new NotImplementedException();
    }


}

