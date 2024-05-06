using PersonalProject.Domain.Entities;

namespace PersonalProject.InternalPortal.Models.Applications;

public class EditApplicationStatusViewModel
{
    public IEnumerable<ApplicationStatus>? ApplicationStatuses { get; set; }
    public string RefNumber { get; set; } = string.Empty;
    public int ApplicationId { get; set; }
    public int StatusId { get; set; }
    public bool? ReviewRecommendation { get; set; }
    public bool? FlaggedForAudit { get; set; }
    public string LastEditedBy { get; set; } = string.Empty;
    public DateTime LastEditedDate { get; set; }
    public string InstallerName { get; set; } = string.Empty;
    public virtual User CurrentContact { get; set; } = new();
    public ApplicationDetail ApplicationDetail { get; set; } = new();
}
