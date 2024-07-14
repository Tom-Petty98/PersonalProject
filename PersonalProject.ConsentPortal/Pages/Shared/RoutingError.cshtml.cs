using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PersonalProject.ConsentPortal.Pages.Shared;

public class RoutingErrorModel : PageModel
{
    public string ErrorTitle { get; set; } = "";
    public string ErrorContent { get; set; } = "";

    public IActionResult OnGet(int statusCode)
    {
        switch(statusCode)
        {
            case 404:
                ErrorTitle = "Page Not Found";
                ErrorContent = "Please click on the link the email.";
                break;
            case 500:
                ErrorTitle = "Sorry, there is a problem with the service";
                ErrorContent = "Try again later";
                break;
            default:
                ErrorTitle = "Sorry, the service is unavailable";
                ErrorContent = "Try again later";
                break;
        }

        return Page();
    }
}
