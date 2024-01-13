using Microsoft.AspNetCore.Mvc;
using PersonalProject.InternalPortal.Services.Installers;

namespace PersonalProject.InternalPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGetInstallerService _getInstallerService;

    public HomeController(ILogger<HomeController> logger, IGetInstallerService getInstallerService)
    {
        _logger = logger;
        _getInstallerService = getInstallerService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _getInstallerService.GetAllInstallersDashboardView();
        return View("InstallerDashboard", model);
    }
}