using PersonalProject.Domain.Entities;

namespace PersonalProject.InternalPortal.Models.Users;

public class UserDetailsViewModel
{
    public int InstallerId { get; set; }
    public string InstallerName { get; set; } = "";
    public string RefNumber { get; set; } = "";
    public List<User> Users { get; set; } = new();
}
