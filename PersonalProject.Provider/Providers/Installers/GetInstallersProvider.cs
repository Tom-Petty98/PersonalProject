using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Provider.Providers.Installers;

public interface IGetInstallersProvider
{
    Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber);
    Task<Installer?> GetInstallerById(int installerId);
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
        return await _context.InstallerStatuses.AsNoTracking()
            .OrderBy(x => x.SortOrder).ToListAsync().ConfigureAwait(false);
    }

    public async Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber)
    {
        return await _context.Installers
            .Include(x => x.Status)
            .Include(x => x.InstallerDetail)
            .Include(x => x.InstallerDetail.InstallerAddress)
            .FirstOrDefaultAsync(x => x.RefNumber == refNumber);
    }

    public async Task<Installer?> GetInstallerById(int installerId)
    {
        return await _context.Installers
            .Include(x => x.Status)
            .Include(x => x.InstallerDetail)
            .Include(x => x.InstallerDetail.InstallerAddress)
            .FirstOrDefaultAsync(x => x.Id == installerId);
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

