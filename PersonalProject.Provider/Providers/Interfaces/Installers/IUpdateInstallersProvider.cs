using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Providers.Interfaces.Installers;

public interface IUpdateInstallersProvider
{
    Task<Installer> AddInstaller(Installer installer);

    Task<bool> UpdateApplication(Installer installer);

    Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail);
}
