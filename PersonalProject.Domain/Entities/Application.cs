namespace PersonalProject.Domain.Entities;

public class Application
{
    public int Id { get; set; }
    public string RefNumber { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public bool? FlaggedForAudit { get; set; }
    public bool? ReviewRecommendation { get; set; }
    public int InstallerId { get; set; }
    public int ApplicationDetailId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public virtual ApplicationStatus? Status { get; set; }
    public virtual ApplicationDetail ApplicationDetail { get; set; }
}

