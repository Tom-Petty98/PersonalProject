using PersonalProject.Domain.Entities;
using PersonalProject.Provider.Providers.Applications;

namespace PersonalProject.CoreAPI.Services.Applications;

public interface IUpdateApplicationService
{
    Task<Application> AddApplication(Application application);
    Task<bool> UpdateApplication(Application application);
}

public class UpdateApplicationService : IUpdateApplicationService
{
    private readonly IUpdateApplicationsProvider _updateApplicationsProvider;

    public UpdateApplicationService(IUpdateApplicationsProvider applicationsProvider)
    {
        _updateApplicationsProvider = applicationsProvider;
    }

    public async Task<Application> AddApplication(Application application)
    => await _updateApplicationsProvider.AddApplication(application);

    public async Task<bool> UpdateApplication(Application application)
        => await _updateApplicationsProvider.UpdateApplication(application);
}
