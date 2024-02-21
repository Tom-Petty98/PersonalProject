using PersonalProject.Domain.Entities;

namespace PersonalProject.InternalPortal.Models.Installers;

public class EditInstallerStatusViewModel
{
    public IEnumerable<InstallerStatus>? InstallerStatuses { get; set; }
    public string RefNumber { get; set; } = string.Empty;
    public int InstallerId { get; set; }
    public int StatusId { get; set; }
    public bool? ReviewRecommendation { get; set; }
    public bool? FlaggedForAudit { get; set; }
    public string LastEditedBy { get; set; } = string.Empty;
    public DateTime LastEditedDate { get; set; }
    public InstallerDetail? InstallerDetail { get; set; }
}
