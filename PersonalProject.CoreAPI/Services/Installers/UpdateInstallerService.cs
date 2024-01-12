using PersonalProject.Domain.Entities;
using PersonalProject.Provider.Providers.Installers;

namespace PersonalProject.CoreAPI.Services.Installers;

public interface IUpdateInstallerService
{
    Task<Installer> AddInstaller(Installer installer);
    Task<bool> UpdateInstaller(Installer installer);
}
public class UpdateInstallerService : IUpdateInstallerService
{
    private readonly IUpdateInstallersProvider _updateInstallersProvider;

    public UpdateInstallerService(IUpdateInstallersProvider InstallersProvider)
    {
        _updateInstallersProvider = InstallersProvider;
    }

    public async Task<Installer> AddInstaller(Installer Installer)
    => await _updateInstallersProvider.AddInstaller(Installer);

    public async Task<bool> UpdateInstaller(Installer Installer)
        => await _updateInstallersProvider.UpdateInstaller(Installer);
}
