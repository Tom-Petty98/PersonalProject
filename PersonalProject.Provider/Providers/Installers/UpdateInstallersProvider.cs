using Microsoft.EntityFrameworkCore;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using System.Diagnostics;


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
                var nextInsNumber = globalsettings.NextInstallerNumber;

                globalsettings.NextInstallerNumber += 1;

                _context.GlobalSettings.Update(globalsettings);

                installer.RefNumber = $"Ins{nextInsNumber}";
                var addedInstaller = _context.Installers.Add(installer).Entity;
                await _context.SaveChangesAsync();

                UpsertStatusHistory(addedInstaller);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            });
        return installer;
    }

    public async Task<bool> UpdateInstaller(Installer installer)
    {
        var foundInstaller = await _context.Installers.FindAsync(installer.Id);

        if (foundInstaller == null) throw new BadRequestException("Installer not found", System.Net.HttpStatusCode.NotFound);

        int result = 0;
        var executionStratergy = _context.Database.CreateExecutionStrategy();
        await executionStratergy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                if (foundInstaller.StatusId != installer.StatusId)
                {
                    UpsertStatusHistory(installer);
                }
                _context.Entry(foundInstaller).CurrentValues.SetValues(installer);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            });
        return result > 0;
    }

    public async Task<bool> UpdateInstallerDetail(InstallerDetail installerDetail)
    {
        _context.InstallerDetails.Update(installerDetail);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    private void UpsertStatusHistory(Installer installer)
    {
        var currentStatusHistory = _context.InstallerStatusHistories.FirstOrDefault(x => x.EndDate == null);

        if (currentStatusHistory != null)
        {
            currentStatusHistory.EndDate = DateTime.UtcNow;
            _context.InstallerStatusHistories.Update(currentStatusHistory);
        }

        var installerStatusHistory = new InstallerStatusHistory
        {
            InstallerId = installer.Id,
            InstallerStatusId = installer.StatusId,
            StartDate = DateTime.UtcNow,
            StatusChangedBy = installer.LastUpdatedBy ?? installer.CreatedBy
        };
        _context.InstallerStatusHistories.Add(installerStatusHistory);
    }
}
