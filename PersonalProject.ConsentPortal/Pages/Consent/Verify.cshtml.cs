using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Services;
using PersonalProject.ConsentPortal.Services.Extensions;

namespace PersonalProject.ConsentPortal.Pages.Consent;

public class VerifyModel : PageModel
{
    private readonly IConsentService _consentService;
    private readonly ISessionAuthorizationService _sessionAuthorizationService;

    public VerifyModel(IConsentService consentService, ISessionAuthorizationService sessionAuthorizationService)
    {
        _consentService = consentService;
        _sessionAuthorizationService = sessionAuthorizationService;
    }

    public async Task<IActionResult> OnGetAsync(string token)
    {
        if (string.IsNullOrEmpty(token)) return NotFound();

        var tokenResult = await _consentService.VerifyToken(token);

        if(tokenResult.TokenAccepted == false || tokenResult.EntityRef == null)
        {
            return RedirectToPage("./Dropout", new { dropoutEnum = DropoutEnum.SessionExpired });
        }
        else if (tokenResult.ExpiryDate < DateTime.UtcNow)
        {
            return RedirectToPage("./Dropout", new { dropoutEnum = DropoutEnum.LinkExpired });
        }

        var consentDetails = await _consentService.GetConsentDetails(tokenResult.EntityRef);

        if (consentDetails == null) 
        {
            return RedirectToPage("./Dropout", new { dropoutEnum = DropoutEnum.SessionExpired });
        }
        else if (consentDetails.HasConsented)
        {
            return RedirectToPage("./Dropout", new { dropoutEnum = DropoutEnum.AlreadyGiven });
        }

        var sessionToken = _sessionAuthorizationService.GenerateSessionToken(consentDetails.AppRefNumber)!;
        HttpContext.Session.Put(Constants.ConsentDetailsSessionKey, consentDetails);
        HttpContext.Session.SetString(Constants.SessionAuthorizationTokenKey, sessionToken);

        return RedirectToPage("./Details");
    }
}
