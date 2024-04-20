
namespace PersonalProject.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsInternalRole { get; set; }
    public virtual ICollection<User>? Users { get; }
}
