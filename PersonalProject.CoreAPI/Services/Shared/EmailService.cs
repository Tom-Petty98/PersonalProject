using PersonalProject.Domain.Request;
using System.Net;
using System.Net.Mail;

namespace PersonalProject.CoreAPI.Services.Shared;

public interface IEmailService
{
    Task SendEmail(EmailDto email);
}

public class EmailService : IEmailService
{
    private readonly Config _config;

    public EmailService(Config config)
    {
        _config = config;
    }

    public async Task SendEmail(EmailDto email)
    {
        var client = new SmtpClient("smtp.ethereal.email", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_config.EmailFrom, _config.EmailPassword)
        };

        await client.SendMailAsync(
            new MailMessage(
                    from: _config.EmailFrom,
                    to: email.RecipientEmail,
                    email.Subject,
                    email.Message
                ));
    }
}
