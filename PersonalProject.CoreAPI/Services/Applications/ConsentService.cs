using PersonalProject.CoreAPI.Services.Shared;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.Provider.Providers.Applications;
using PersonalProject.Provider.Providers.Installers;

namespace PersonalProject.CoreAPI.Services.Applications;

public interface IConsentService
{
    Task SendConsentEmail(string refNumber);
    Task<ConsentDetails> GetConsentDetails(string appRefNumber);
    Task<bool> RegisterConsent(string appRefNumber);
}

public class ConsentService : IConsentService
{
    private readonly IEmailService _emailService;
    private readonly IGetApplicationsProvider _getApplicationsProvider;
    private readonly IUpdateApplicationsProvider _updateApplicationsProvider;
    private readonly IGetInstallersProvider _getInstallerProvider;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly Config _config;

    public ConsentService(IEmailService emailService,
        IGetApplicationsProvider getApplicationsProvider,
        IUpdateApplicationsProvider updateApplicationsProvider,
        IGetInstallersProvider getInstallerProvider,
        IJwtTokenService jwtTokenService,
        Config config)
    {
        _emailService = emailService;
        _getApplicationsProvider = getApplicationsProvider;
        _updateApplicationsProvider = updateApplicationsProvider;
        _getInstallerProvider = getInstallerProvider;
        _jwtTokenService = jwtTokenService;
        _config = config;
    }

    public async Task<ConsentDetails> GetConsentDetails(string appRefNumber)
    {
        var app = await _getApplicationsProvider.GetApplicationByReferenceNumberAsync(appRefNumber);
        var installerName = await _getInstallerProvider.GetInstallerNameById(app!.InstallerId);
        var techTypes = await _getApplicationsProvider.GetTechTypesAsync();
        var techTypeDescription = techTypes.First(x => x.Id == app.ApplicationDetail.TechTypeId).Description;

        return new ConsentDetails
        {
            AppRefNumber = appRefNumber,
            HasConsented = app.ApplicationDetail.ConsentRecieved ?? false,
            TechTypeDescription = techTypeDescription,
            InstallerName = installerName,
            AddressLine1 = app.ApplicationDetail.InstallationAddress!.AddressLine1,
            Postcode = app.ApplicationDetail.InstallationAddress.Postcode,
        };
    }

    public async Task SendConsentEmail(string refNumber)
    {
        var app = await _getApplicationsProvider.GetApplicationByReferenceNumberAsync(refNumber);

        if (app == null || app.ApplicationDetail == null || app.ApplicationDetail.InstallationAddress == null 
            || app.ApplicationDetail.PropertyOwnerEmail == null) 
        {
            throw new BadRequestException("Could not find application or property owner email");
        }

        //duplicate uprn logic here
        var email = await GenerateEmail(app);    

        await _emailService.SendEmail(email);
    }

    private async Task<EmailDto> GenerateEmail(Application app)
    {
        var tokenExpiry = DateTime.UtcNow.AddDays(_config.ConsentExpiryDays);
        var timeSpan = new TimeSpan(23, 59, 59);
        tokenExpiry = tokenExpiry.Date + timeSpan;

        var token = _jwtTokenService.GenerateToken(app.RefNumber, tokenExpiry);
        var consentTokenUrl = $"{_config.ConsentPortalBaseAddress}verify?token={token}";

        var installerName = await _getInstallerProvider.GetInstallerNameById(app.InstallerId);
        var installationAddress = app.ApplicationDetail.InstallationAddress;

        return new EmailDto
        {
            RecipientEmail = app.ApplicationDetail.PropertyOwnerEmail,
            Message = $"The installer {installerName} has made an application for an installation at: \n\n" +
            $"{installationAddress!.AddressLine1} \n\n {installationAddress.Postcode} \n\n" +
            $"In order to progress the above installation at this address your consent is required please click the link below to give consent. \n\n" +
            $"{consentTokenUrl}"
        };
    }

    public async Task<bool> RegisterConsent(string appRefNumber)
    {
        var app = await _getApplicationsProvider.GetApplicationByReferenceNumberAsync(appRefNumber);
        var appDetail = app!.ApplicationDetail;
        appDetail.ConsentRecieved = true;

        return await _updateApplicationsProvider.UpdateApplicationDetail(appDetail);
    }
}
