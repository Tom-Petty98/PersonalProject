namespace PersonalProject.Domain.Entities;

public class Installer
{
    public int Id { get; set; }
    public string RefNumber { get; set; } = string.Empty;
    public int InstallerDetailId {  get; set; }
    public int StatusId { get; set; }
    public bool? FlaggedForAudit { get; set; }
    public bool? ReviewRecommendation { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public virtual InstallerStatus? Status { get; set; }
    public virtual InstallerDetail InstallerDetail { get; set; }
}
