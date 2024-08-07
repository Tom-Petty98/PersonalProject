﻿using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.Provider.Providers.Installers;

namespace PersonalProject.CoreAPI.Services.Installers;

public interface IGetInstallersService
{
    Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber);
    Task<Installer?> GetInstallerByIdAsync(int installerId);
    Task<string> GetInstallerNameByIdAsync(int installerId);
    Task<IEnumerable<InstallerDashboard>> GetAllInstallersDashboardView();
    Task<PagedResult<InstallerDashboard>> GetPagedInstallers(DashboardFilter dashboardFilter);
    Task<IEnumerable<InstallerStatus>> GetAllInstallerStatusesAsync();
}


public class GetInstallersService : IGetInstallersService
{
    private readonly IGetInstallersProvider _getInstallersProvider;

    public GetInstallersService(IGetInstallersProvider installersProvider)
    {
        _getInstallersProvider = installersProvider;
    }

    public async Task<IEnumerable<InstallerDashboard>> GetAllInstallersDashboardView()
    {
        return await _getInstallersProvider.GetAllInstallersDashboardView();
    }

    public async Task<IEnumerable<InstallerStatus>> GetAllInstallerStatusesAsync()
        => await _getInstallersProvider.GetAllInstallerStatusesAsync();

    public async Task<Installer?> GetInstallerByReferenceNumberAsync(string refNumber)
     => await _getInstallersProvider.GetInstallerByReferenceNumberAsync(refNumber);

    public async Task<Installer?> GetInstallerByIdAsync(int installerId)
     => await _getInstallersProvider.GetInstallerById(installerId);

    public async Task<string> GetInstallerNameByIdAsync(int installerId)
     => await _getInstallersProvider.GetInstallerNameById(installerId);

    public Task<PagedResult<InstallerDashboard>> GetPagedInstallers(DashboardFilter dashboardFilter)
    {
        throw new NotImplementedException();
    }
}
