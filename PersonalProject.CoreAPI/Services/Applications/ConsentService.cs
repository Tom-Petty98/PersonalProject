using PersonalProject.CoreAPI.Services.Shared;
using PersonalProject.Domain.Request;

namespace PersonalProject.CoreAPI.Services.Applications;

public interface IConsentService
{
    Task SendConsentEmail(string refNumber);
}

public class ConsentService : IConsentService
{
    private readonly IEmailService _emailService;
    private readonly IGetApplicationsService _getApplicationsService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly Config _config;

    public ConsentService(IEmailService emailService,
        IGetApplicationsService getApplicationsService,
        IJwtTokenService jwtTokenService,
        Config config)
    {
        _emailService = emailService;
        _getApplicationsService = getApplicationsService;
        _jwtTokenService = jwtTokenService;
        _config = config;
    }

    public async Task SendConsentEmail(string refNumber)
    {
        var app = await _getApplicationsService.GetApplicationByReferenceNumberAsync(refNumber);

        if (app == null || app.ApplicationDetail == null || app.ApplicationDetail.PropertyOwnerEmail == null) 
        {
            throw new BadRequestException("Could not find application or property owner email");
        }

        //duplicate uprn logic here

        var email = GenerateEmail(app.ApplicationDetail.PropertyOwnerEmail, app.Id);
        

        await _emailService.SendEmail(email);
    }

    private EmailDto GenerateEmail(string propertyOwnerEmail, int appId)
    {
        var tokenExpiry = DateTime.UtcNow.AddDays(_config.ConsentExpiryDays);
        var timeSpan = new TimeSpan(23, 59, 59);
        tokenExpiry = tokenExpiry.Date + timeSpan;

        var token = _jwtTokenService.GenerateToken(appId, tokenExpiry);

        //var consentTokenUrl = $"{_config.ConsentPortalBaseAddress}verify?token{token}";
        return new EmailDto
        {
            RecipientEmail = propertyOwnerEmail,
            Message = "This is a test email"
        };
    }
}
