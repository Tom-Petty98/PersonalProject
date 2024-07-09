namespace PersonalProject.Domain.Request;

public class EmailDto
{
    public string RecipientEmail { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Message { get; set; } = "";
}
