namespace PersonalProject.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public int InstallerId { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid? AzureId { get; set; }
    public int RoleId { get; set; }
    public bool IsObselete { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public virtual UserRole Role { get; set; }
}
