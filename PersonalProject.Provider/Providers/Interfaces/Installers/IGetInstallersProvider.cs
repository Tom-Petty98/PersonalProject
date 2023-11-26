using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;

namespace PersonalProject.Provider.Providers.Interfaces.Installers;

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
