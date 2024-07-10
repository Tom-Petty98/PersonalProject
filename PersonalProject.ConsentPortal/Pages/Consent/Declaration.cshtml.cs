using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Services;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;

namespace PersonalProject.ConsentPortal.Pages.Consent;

public class DeclarationModel : PageModel
{
    private readonly IConsentService _consentService;

    [BindProperty]
    public bool Requirement1 { get; set; }

    [BindProperty]
    public bool Requirement2 { get; set; }
    [BindProperty]
    public bool Requirement3 { get; set; }
    [BindProperty]
    public bool Requirement4 { get; set; }

    public DeclarationModel(IConsentService consentService)
    {
        _consentService = consentService;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!Requirement1 || !Requirement2 || !Requirement3 || !Requirement4)
        {
            ModelState.AddModelError(nameof(Requirement1), "Ensure all declarations have been confirmed");
            return OnGet();
        }

        var consentDetails = HttpContext.Session.GetOrDefault<ConsentDetails>(Constants.ConsentDetailsSessionKey)!;
        //await _consentService.RegisterCosent(consentDetails.AppRefNumber);

        return RedirectToPage("./Confirmation");
    }
}
