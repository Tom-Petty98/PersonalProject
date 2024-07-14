using Microsoft.AspNetCore.Mvc;
using PersonalProject.CoreAPI.Services.Applications;
using PersonalProject.CoreAPI.Services.Shared;
using PersonalProject.Domain.Request;

namespace PersonalProject.CoreAPI.Controllers.Applications;

[ApiController]
[Route("Consent")]
public class ConsentController : ControllerBase
{
    private readonly IConsentService _consentService;
    private readonly IJwtTokenService _jwtTokenService;

    public ConsentController(IConsentService consentService, IJwtTokenService jwtTokenService)
    {
        _consentService = consentService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    [Route("SendConsentEmail/{refNumber}")]
    public async Task<IActionResult> SendConsentEmail(string refNumber)
    {
        await _consentService.SendConsentEmail(refNumber);
        return Ok();
    }

    [HttpGet]
    [Route("VerifyToken/{token}")]
    [ProducesResponseType(typeof(TokenVerificationResult), 200)]
    public IActionResult VerifyToken(string token)
    {
        return Ok(_jwtTokenService.VerifyToken(token));
    }

    [HttpGet]
    [Route("GetConsentDetails/{appRefNumber}")]
    [ProducesResponseType(typeof(ConsentDetails), 200)]
    public async Task<IActionResult> GetConsentDetails(string appRefNumber)
    {
        return Ok(await _consentService.GetConsentDetails(appRefNumber));
    }

    [HttpPost]
    [Route("RegisterConsent/{refNumber}")]
    public async Task<IActionResult> RegisterConsent(string refNumber)
    {
        await _consentService.RegisterConsent(refNumber);
        return Ok();
    }
}