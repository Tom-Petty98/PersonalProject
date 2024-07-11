using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalProject.ConsentPortal.Models;
using PersonalProject.ConsentPortal.Services.Extensions;
using PersonalProject.Domain.Request;

namespace PersonalProject.ConsentPortal.Pages.Consent;

[ServiceFilter(typeof(SessionTokenAuthorizeAttribute))]
public class ConfirmationModel : PageModel
{
    public ConsentDetails ConsentDetails { get; set; } = null!;

    public IActionResult OnGet()
    {
        ConsentDetails = HttpContext.Session.GetOrDefault<ConsentDetails>(Constants.ConsentDetailsSessionKey)!;

        return Page();
    }
}
