namespace PersonalProject.Domain.Entities;

public class InstallerStatusHistory
{
    public int Id { get; set; }
    public int InstallerId { get; set; }
    public int InstallerStatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StatusChangedBy { get; set; } = string.Empty;
    public virtual ApplicationStatus ApplicationStatus { get; set; }
}