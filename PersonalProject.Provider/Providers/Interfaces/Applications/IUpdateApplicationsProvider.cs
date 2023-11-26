using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Providers.Interfaces.Applications;

public interface IUpdateApplicationsProvider
{
    Task<Application> AddApplication(Application application);

    Task<bool> UpdateApplication(Application application);

    Task<bool> UpdateApplicationDetail(ApplicationDetail applicationDetail);
}
