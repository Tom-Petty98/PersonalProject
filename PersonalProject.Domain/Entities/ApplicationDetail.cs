namespace PersonalProject.Domain.Entities;

public class ApplicationDetail
{
    public int Id { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public int TechTypeId { get; set; }
    public int InstallationAddressId { get; set; }
    public string PropertyOwnerEmail { get; set; } = string.Empty;
    public string? EpcNumber { get; set; }
    public bool? ConsentRecieved { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public virtual Address? InstallationAddress { get; set; }
    public virtual TechType? TechType { get; set; }
}
