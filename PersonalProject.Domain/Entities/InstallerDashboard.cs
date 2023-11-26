﻿namespace PersonalProject.Domain.Entities;
public class InstallerDashboard
{
    public string? RefNumber { get; set; }
    public string? InstallerName { get; set; }
    public string? StatusDescription { get; set; }
    public string? StatusCode { get; set; }
    public bool? ReviewRecommendation { get; set; }
    public bool? FlaggedForAudit { get; set; }
    public DateTime? LastStatusChangeDate { get; set; }
}