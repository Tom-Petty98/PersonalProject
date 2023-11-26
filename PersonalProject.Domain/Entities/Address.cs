using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalProject.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public string Postcode { get; set; } = string.Empty;
    public int UPRN { get; set; } 
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
}