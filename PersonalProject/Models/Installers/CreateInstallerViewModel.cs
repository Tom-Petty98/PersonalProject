using System.ComponentModel.DataAnnotations;

namespace PersonalProject.InternalPortal.Models.Installers;

public class CreateInstallerViewModel
{
    public bool? FlaggedForAudit { get; set; }

    [Required(ErrorMessage = "Installer name is required"), StringLength(255, MinimumLength = 2)]
    [RegularExpression(@"^[A-Z][a-zA-Z""'\s-]*$")]
    public string InstallerName { get; set; } = string.Empty;
    public int CompanyNumber { get; set; }

    public string Postcode { get; set; } = string.Empty;
    public int UPRN { get; set; }
    [Required(ErrorMessage = "Address Line 1 is required"), StringLength(255, MinimumLength = 2)]
    [RegularExpression(@"^[A-Z][a-zA-Z""'\s-]*$")]
    public string AddressLine1 { get; set; } = string.Empty;
    [StringLength(255, MinimumLength = 2)]
    public string? AddressLine2 { get; set; }
    [StringLength(127, MinimumLength = 2)]
    public string? AddressLine3 { get; set; }
}
