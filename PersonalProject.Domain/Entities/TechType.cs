
namespace PersonalProject.Domain.Entities;

public class TechType
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
