namespace PersonalProject.CoreAPI.Services.Shared;

public class Config
{
    public string EmailPassword { get; set; } = null!;
    public string EmailFrom { get; set; } = null!;
    public int ConsentExpiryDays { get; set; }
    public Uri ConsentPortalBaseAddress { get; set; } = null!;
    public string EmailTokenSecret { get; set; } = null!;
}
