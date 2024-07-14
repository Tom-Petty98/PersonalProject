using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalProject.ConsentPortal.Models;

namespace PersonalProject.ConsentPortal.Pages.Consent;

public class DropoutModel : PageModel
{
    private Dictionary<DropoutEnum, string> _dropoutHeading = new Dictionary<DropoutEnum, string>
    {
        { DropoutEnum.SessionExpired, "Session expired" },
        { DropoutEnum.AlreadyGiven, "Consent already given" },
        { DropoutEnum.LinkExpired, "The email link has expired" },
    };

    private Dictionary<DropoutEnum, string> _dropoutContent = new Dictionary<DropoutEnum, string> 
    {
        { DropoutEnum.SessionExpired, "If you still need to give consent please click on the email link again." },
        { DropoutEnum.AlreadyGiven, "You have already give consent for this applicaiton." },
        { DropoutEnum.LinkExpired, "The email link has expired you will need to let your installer know so that consent can be reissued." },
    };
    private readonly ILogger<DropoutModel> _logger;

    public string Heading { get; set; } = "";
    public string BodyContent { get; set; } = "";

    public DropoutModel(ILogger<DropoutModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet(DropoutEnum? dropoutEnum)
    {
        DropoutEnum dropoutEnum2 = dropoutEnum ?? DropoutEnum.SessionExpired;

        Heading = _dropoutHeading[dropoutEnum2];
        BodyContent = _dropoutContent[dropoutEnum2];

        _logger.LogInformation($"DropoutModel OnGet, {Heading}");

        return Page();
    }
}
