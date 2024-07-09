namespace PersonalProject.Domain.Request;

public class TokenVerificationResult
{
    public bool TokenAccepted {  get; set; }
    public int EntityId { get; set; }
    public DateTime ExpiryDate { get; set; }
}
