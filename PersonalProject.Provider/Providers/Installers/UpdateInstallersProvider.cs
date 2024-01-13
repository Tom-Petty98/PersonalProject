using Microsoft.EntityFrameworkCore;
using PersonalProject.Domain.Entities;


namespace PersonalProject.Provider.Providers.Installers;

public interface IUpdateInstallersProvider
{
    Task<Installer> AddInstaller(Installer installer);

    Task<bool> UpdateInstaller(Installer installer);

    Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail);
}

public class UpdateInstallersProvider : IUpdateInstallersProvider
{
    private readonly ApplicationDbContext _context;
    public UpdateInstallersProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Installer> AddInstaller(Installer installer)
    {
        var executionStratergy = _context.Database.CreateExecutionStrategy();
        await executionStratergy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var globalsettings = _context.GlobalSettings.First();
                var nextAppNumber = globalsettings.NextInstallerNumber;

                globalsettings.NextInstallerNumber += 1;

                _context.GlobalSettings.Update(globalsettings);

                installer.RefNumber = $"App{nextAppNumber}";
                var id = _context.Installers.Add(installer);


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            });
        return installer;
    }

    public async Task<bool> UpdateInstaller(Installer installer)
    {
        try
        {
            _context.Attach(installer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail)
    {
        throw new NotImplementedException();
    }
}
