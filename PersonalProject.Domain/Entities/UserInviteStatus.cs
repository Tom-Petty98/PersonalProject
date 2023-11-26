using static PersonalProject.Domain.Enums.UserInviteStatus;

namespace PersonalProject.Domain.Entities;

public partial class UserInviteStatus
{
    public int Id { get; set; }
    public string Description { get; set; }
    public UserInviteStatusCode Code { get; set; }
}
