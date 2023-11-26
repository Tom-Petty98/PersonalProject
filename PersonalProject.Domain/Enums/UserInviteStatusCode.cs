namespace PersonalProject.Domain.Enums;

public partial class UserInviteStatus
{
    public enum UserInviteStatusCode
    {
        INVITED,
        SIGNEDUP,
        EXPIRED,
        CANCELLED,
        NOTSENT,
        NOTDELIVRD
    }
}