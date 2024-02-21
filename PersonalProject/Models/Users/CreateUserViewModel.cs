using System.ComponentModel.DataAnnotations;

namespace PersonalProject.InternalPortal.Models.Users;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Enter an email adress")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public int InstallerId {  get; set; }
}
