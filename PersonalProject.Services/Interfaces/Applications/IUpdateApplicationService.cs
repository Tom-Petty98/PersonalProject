using PersonalProject.Domain.Entities;

namespace PersonalProject.Services.Interfaces.Applications;

public interface IUpdateApplicationService
{
    Task<Application> AddApplication(Application application);
    Task<bool> UpdateApplication(Application application);
}
