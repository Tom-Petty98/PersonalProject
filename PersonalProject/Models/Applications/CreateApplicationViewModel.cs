﻿using PersonalProject.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalProject.InternalPortal.Models.Applications;

public class CreateApplicationViewModel
{
    public IEnumerable<TechType>? TechTypes { get; set; }
    public int UserId { get; set; }
    public int InstallerId { get; set; }
    public string InstallerName { get; set; } = string.Empty;
    public bool? FlaggedForAudit { get; set; }
    [Required]
    public DateTime? SubmittedDate { get; set; }
    [Required]
    public int TechTypeId { get; set; }
    public string? EpcNumber { get; set; }
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
}
