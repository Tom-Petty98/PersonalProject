using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;

namespace PersonalProject.InternalPortal.Services.Interfaces;

public interface IInstallerService
{
    Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber);
    Task<IEnumerable<InstallerDashboard>> GetAllInstallersDashboardView();

    Task<IEnumerable<InstallerStatus>> GetAllInstallerStatusesAsync();

    Task<PagedResult<InstallerDashboard>> GetPagedInstallers(DashboardFilter dashboardFilter);

    Task<Installer> AddInstaller(Installer installer);

    Task<bool> UpdateInstaller(Installer installer);

    Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail);
}
