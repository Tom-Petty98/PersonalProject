using Microsoft.EntityFrameworkCore;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;


namespace PersonalProject.Provider.Providers.Applications;

public interface IUpdateApplicationsProvider
{
    Task<Application> AddApplication(Application application);
    Task<bool> UpdateApplication(Application application);
    Task<bool> UpdateApplicationDetail(ApplicationDetail applicationDetail);
}

public class UpdateApplicationsProvider : IUpdateApplicationsProvider
{
    private readonly ApplicationDbContext _context;
    public UpdateApplicationsProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Application> AddApplication(Application application)
    {
        var executionStratergy = _context.Database.CreateExecutionStrategy();
        await executionStratergy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var globalsettings = await _context.GlobalSettings.FirstAsync();
                var nextAppNumber = globalsettings!.NextAppNumber;

                globalsettings.NextAppNumber += 1;

                _context.GlobalSettings.Update(globalsettings);

                application.RefNumber = $"App{nextAppNumber}";
                var addedApp = _context.Applications.Add(application).Entity;
                await _context.SaveChangesAsync();

                await UpsertStatusHistory(addedApp);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            });
        return application;
    }

    public async Task<bool> UpdateApplication(Application application)
    {
        var foundapplication = await _context.Applications.FindAsync(application.Id);

        if (foundapplication == null) throw new BadRequestException("application not found", System.Net.HttpStatusCode.NotFound);

        int result = 0;
        var executionStratergy = _context.Database.CreateExecutionStrategy();
        await executionStratergy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                if (foundapplication.StatusId != application.StatusId)
                {
                    await UpsertStatusHistory(application);
                }
                _context.Entry(foundapplication).CurrentValues.SetValues(application);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            });
        return result > 0;
    }

    public async Task<bool> UpdateApplicationDetail(ApplicationDetail applicationDetail)
    {
        _context.ApplicationDetails.Update(applicationDetail);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    private async Task UpsertStatusHistory(Application application)
    {
        var currentStatusHistory = await _context.ApplicationStatusHistories
            .FirstOrDefaultAsync(x => x.ApplicationId == application.Id && x.EndDate == null);

        if (currentStatusHistory != null)
        {
            currentStatusHistory.EndDate = DateTime.UtcNow;
            _context.ApplicationStatusHistories.Update(currentStatusHistory);
        }

        var applicationStatusHistory = new ApplicationStatusHistory
        {
            ApplicationId = application.Id,
            ApplicationStatusId = application.StatusId,
            StartDate = DateTime.UtcNow,
            StatusChangedBy = application.LastUpdatedBy ?? application.CreatedBy
        };
        _context.ApplicationStatusHistories.Add(applicationStatusHistory);
    }
}
