using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;

namespace PersonalProject.ConsentPortal.Pages.Consent;

[ServiceFilter(typeof(SessionTokenAuthorizeAttribute))]
public class DetailsModel : PageModel
{

    [BindProperty]
    public bool InstallerConfirmed { get; set; }

    [BindProperty]
    public bool PropertyDetailsConfirmed { get; set; }

    public ConsentDetails ConsentDetails { get; set; } = null!;

    public IActionResult OnGet()
    {
        ConsentDetails = HttpContext.Session.GetOrDefault<ConsentDetails>(Constants.ConsentDetailsSessionKey)!;

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!InstallerConfirmed)
            ModelState.AddModelError(nameof(InstallerConfirmed), "Confirm installer details are correct");
        if (!PropertyDetailsConfirmed)
            ModelState.AddModelError(nameof(PropertyDetailsConfirmed), "Confirm intallation property details are correct");

        if (!ModelState.IsValid)
            return OnGet();

        return RedirectToPage("./Declaration");
    }
}
