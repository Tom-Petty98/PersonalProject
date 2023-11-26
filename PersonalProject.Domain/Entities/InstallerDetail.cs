namespace PersonalProject.Domain.Entities;

public class InstallerDetail
{
    public int Id { get; set; }
    public string InstallerName { get; set; } = string.Empty;
    public int CompanyNumber { get; set; }
    public int? InstallerAddressId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public virtual Address? InstallerAddress { get; set; }
}
