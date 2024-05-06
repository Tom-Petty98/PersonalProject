using System.ComponentModel.DataAnnotations;

namespace PersonalProject.InternalPortal.Models.Applications;

public class EditApplicationDetailsViewModel
{
    public string ApplicationStatusDescription { get; set; } = string.Empty;
    public string RefNumber { get; set; } = string.Empty;
    public string InstallerName { get; set; } = string.Empty;
    public DateTime? SubmittedDate { get; set; }
    [Required(ErrorMessage = "Property owner email is required"), StringLength(255, MinimumLength = 2)]
    [EmailAddress]
    public string PropertyOwnerEmail { get; set; } = string.Empty;
    public string Postcode { get; set; } = string.Empty;
    public int UPRN { get; set; }
    [Required(ErrorMessage = "Address Line 1 is required"), StringLength(255, MinimumLength = 2)]
    public string AddressLine1 { get; set; } = string.Empty;
    [StringLength(255, MinimumLength = 2)]
    public string? AddressLine2 { get; set; }
    [StringLength(127, MinimumLength = 2)]
    public string? AddressLine3 { get; set; }
    public string LastEditedBy { get; set; } = string.Empty;
    public DateTime LastEditedDate { get; set; }
}
