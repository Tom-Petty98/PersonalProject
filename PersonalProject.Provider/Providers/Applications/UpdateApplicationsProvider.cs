using Microsoft.EntityFrameworkCore;
using PersonalProject.Domain.Entities;
using PersonalProject.Provider.Data;


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

                var globalsettings = _context.GlobalSettings.First();
                var nextAppNumber = globalsettings.NextAppNumber;

                globalsettings.NextAppNumber += 1;

                _context.GlobalSettings.Update(globalsettings);

                application.RefNumber = $"App{nextAppNumber}";
                var id = _context.Applications.Add(application);


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            });
        return application;
    }

    public async Task<bool> UpdateApplication(Application application)
    {
        try
        {
            _context.Attach(application).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public Task<bool> UpdateApplicationDetail(ApplicationDetail applicationDetail)
    {
        throw new NotImplementedException();
    }
}
