using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;

namespace PersonalProject.InternalPortal.Models.Home;

public class AppDashboardViewModel
{
    public IEnumerable<ApplicationStatus>? ApplicationStatuses { get; set; }
    public List<string>? StatusCodesToFilterBy { get; set; }
    public string SearchBy { get; set; } = "";
    public string? SortByColumn { get; set; }
    public bool SortByDesc { get; set; } = false;
    public PagedResult<ApplicationDashboard> Applications { get; set; } = new();
}
