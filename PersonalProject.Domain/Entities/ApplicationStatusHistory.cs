namespace PersonalProject.Domain.Entities;

public class ApplicationStatusHistory
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public int ApplicationStatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StatusChangedBy { get; set; } = string.Empty;
    public virtual ApplicationStatus ApplicationStatus { get; set; }

}