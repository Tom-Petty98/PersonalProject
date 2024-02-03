﻿namespace PersonalProject.Domain.Entities;

public class InstallerDashboard
{
    public string? RefNumber { get; set; }
    public string? InstallerName { get; set; }
    public string? StatusDescription { get; set; }
    public string? StatusCode { get; set; }
    public string? ReviewRecommendation { get; set; }
    public string? FlaggedForAudit { get; set; }
    public DateTime? LastStatusChangeDate { get; set; }
}
