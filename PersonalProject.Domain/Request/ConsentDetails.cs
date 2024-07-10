namespace PersonalProject.Domain.Request;

public class ConsentDetails
{
    public string AppRefNumber { get; set; } = null!;
    public string InstallerName { get; set; } = null!;
    public string TechTypeDescription { get; set; } = null!;
    public string Postcode { get; set; } = null!;
    public string AddressLine1 { get; set; } = null!;
}
