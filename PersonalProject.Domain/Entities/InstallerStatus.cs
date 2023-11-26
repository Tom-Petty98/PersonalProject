
using static PersonalProject.Domain.Enums.InstallerStatus;

namespace PersonalProject.Domain.Entities;

public partial class InstallerStatus
{
    public int Id { get; set; }
    public string Description { get; set; }
    public InstallerStatusCode Code { get; set; }
    public int SortOrder { get; set; }
}