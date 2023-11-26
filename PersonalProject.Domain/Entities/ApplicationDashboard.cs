namespace PersonalProject.Domain.Entities;
public class ApplicationDashboard
{
    public string? AppRefNumber { get; set; }
    public string? Postcode { get; set; }
    public string? StatusDescription { get; set; }
    public string? StatusCode { get; set; }
    public bool? ReviewRecommendation { get; set; }
    public bool? FlaggedForAudit { get; set; }
    public DateTime? LastStatusChangeDate { get; set; }
}