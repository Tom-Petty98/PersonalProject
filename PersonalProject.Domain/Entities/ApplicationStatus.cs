
using static PersonalProject.Domain.Enums.ApplicationStatus;

namespace PersonalProject.Domain.Entities;

public partial class ApplicationStatus
{
    public int Id { get; set; }
    public string Description { get; set; }
    public AppStatusCode Code { get; set; }
    public int SortOrder { get; set; }
}