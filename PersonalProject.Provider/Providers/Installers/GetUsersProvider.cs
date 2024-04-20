using PersonalProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PersonalProject.Provider.Providers.Installers;

public interface IGetUsersProvider
{
    Task<List<User>?> GetUsersByInstallerId(int installerId);
    Task<User?> GetUserById(int userId);
    Task<bool> IsEmailInUse(string email);
    Task<IEnumerable<Role>> GetUserRolesAsync(bool getInternal);
}

public class GetUsersProvider : IGetUsersProvider
{
    private readonly ApplicationDbContext _context;
    public GetUsersProvider(ApplicationDbContext context)
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

    public async Task<User?> GetUserById(int userId)
    {
        return await _context.Users
            .Include("Roles")
            .FirstOrDefaultAsync(x => x.Id == userId);
    }  

    public async Task<List<User>?> GetUsersByInstallerId(int installerId)
    {
        return await _context.Users
            .Include("Roles")
            .Where(x => x.InstallerId == installerId).ToListAsync();
    }

    public async Task<bool> IsEmailInUse(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email && !x.IsObselete);

        return user != null;
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(bool getInternal)
    {
        return await _context.Roles.AsNoTracking()
            .Where(x => x.IsInternalRole == getInternal)
            .OrderBy(x => x.Description).ToListAsync().ConfigureAwait(false);
    }
}

