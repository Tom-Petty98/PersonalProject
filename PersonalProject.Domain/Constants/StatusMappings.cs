using static PersonalProject.Domain.Enums.ApplicationStatus;
using static PersonalProject.Domain.Enums.InstallerStatus;

namespace PersonalProject.Domain.Constants;

public static class StatusMappings
{
    public static readonly Dictionary<InstallerStatusCode, int> InstallerStatuses = new Dictionary<InstallerStatusCode, int>
    {
        { InstallerStatusCode.SUB, 1 }
    };


    public static readonly Dictionary<AppStatusCode, int> ApplicationStatuses = new Dictionary<AppStatusCode, int>
    {
        { AppStatusCode.SUB, 1 }
    };

}
