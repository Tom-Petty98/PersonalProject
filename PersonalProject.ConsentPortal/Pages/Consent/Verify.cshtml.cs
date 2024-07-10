using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Services;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;

namespace PersonalProject.ConsentPortal.Pages.Consent;

public class VerifyModel : PageModel
{
    private readonly IConsentService _consentService;

    public VerifyModel(IConsentService consentService)
    {
        _consentService = consentService;
    }

    public async Task<IActionResult> OnGetAsync(string token)
    {
        if (string.IsNullOrEmpty(token)) return NotFound();

        var tokenResult = await _consentService.VerifyToken(token);

        if(tokenResult.TokenAccepted == false 
            || tokenResult.ExpiryDate <  DateTime.UtcNow
            || tokenResult.EntityRef == null)
        {
            return RedirectToPage("./SessionExpired");
        }

        //setup session
        var consentDetails = await _consentService.GetConsentDetails(tokenResult.EntityRef);
        HttpContext.Session.Put(Constants.ConsentDetailsSessionKey, consentDetails);

        return RedirectToPage("./Details");
    }
}
