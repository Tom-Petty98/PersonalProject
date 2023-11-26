namespace PersonalProject.Domain.Entities;

public class UserInvite
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int UserInviteStatusId { get; set; }
    public DateTime ExpiresOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public virtual User User { get; set; }
    public virtual UserInviteStatus UserInviteStatus { get; set; }
}
