namespace PersonalProject.Domain.Request;

public class TokenVerificationResult
{
    public bool TokenAccepted {  get; set; }
    public string EntityRef { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
}
