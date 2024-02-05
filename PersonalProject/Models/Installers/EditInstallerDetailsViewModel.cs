using PersonalProject.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalProject.InternalPortal.Models.Installers;

public class EditInstallerDetailsViewModel
{
    public string InstallerStatusDescription { get; set; } = string.Empty;
    public string RefNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "Installer name is required"), StringLength(255, MinimumLength = 2)]
    [RegularExpression(@"^[A-Z][a-zA-Z""'\s-]*$")]
    public string InstallerName { get; set; } = string.Empty;
    public int CompanyNumber { get; set; }
    //public bool InstallerHasAddress { get; set; } = false;
    [Required(ErrorMessage = "Postcode is required")]
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
