using PersonalProject.Domain.Entities;

namespace PersonalProject.Domain.Request;

public class DashboardFilter
{
    public int PageNum { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "RefNumber";
    public bool OrderByDescending { get; set; } = true;
    public string SearchBy { get; set; } = "";
    public List<string>? FilteredAppStatuses {  get; set; }
}
