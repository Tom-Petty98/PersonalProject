using PersonalProject.Domain.Entities;
using PersonalProject.Provider.Providers.Installers;

namespace PersonalProject.CoreAPI.Services.Installers;

public interface IUpdateInstallerService
{
    Task<Installer> AddInstaller(Installer installer);
    Task<bool> UpdateInstaller(Installer installer);
    Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail);
}
public class UpdateInstallerService : IUpdateInstallerService
{
    private readonly IUpdateInstallersProvider _updateInstallersProvider;

    public UpdateInstallerService(IUpdateInstallersProvider InstallersProvider)
    {
        _updateInstallersProvider = InstallersProvider;
    }

    public async Task<Installer> AddInstaller(Installer installer)
    => await _updateInstallersProvider.AddInstaller(installer);

    public async Task<bool> UpdateInstaller(Installer installer)
        => await _updateInstallersProvider.UpdateInstaller(installer);

    public async Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail)
        => await _updateInstallersProvider.UpdateInstallerDetail(installerDetail);
}
