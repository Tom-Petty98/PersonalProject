using PersonalProject.Domain.Entities;
using PersonalProject.Provider.Providers.Interfaces.ApplicationsProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalProject.Services.Implementation.Applications;

public class UpdateApplicationService
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
