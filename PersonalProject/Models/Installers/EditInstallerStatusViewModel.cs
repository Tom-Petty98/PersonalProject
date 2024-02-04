namespace PersonalProject.InternalPortal.Models.Installers;

public class EditInstallerStatusViewModel
{
    public string RefNumber { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public bool? ReviewRecommendation { get; set; }
    public bool? FlaggedForAudit { get; set; }
    public string LastEditedBy { get; set; } = string.Empty;
    public DateTime LastEditedDate { get; set; }
}
