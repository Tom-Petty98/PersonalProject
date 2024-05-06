using Microsoft.AspNetCore.Mvc;
using PersonalProject.InternalPortal.Services.Applications;
using PersonalProject.InternalPortal.Services.Installers;

namespace PersonalProject.InternalPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGetInstallerService _getInstallerService;
    private readonly IGetApplicationsService _getApplicationsService;

    public HomeController(ILogger<HomeController> logger,
        IGetInstallerService getInstallerService,
        IGetApplicationsService getApplicationsService)
    {
        _logger = logger;
        _getInstallerService = getInstallerService;
        _getApplicationsService = getApplicationsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _getInstallerService.GetAllInstallersDashboardView();
        return View("InstallerDashboard", model);
    }

    [HttpGet]
    public async Task<IActionResult> ApplicationDashboard()
    {
        var model = await _getApplicationsService.GetAllApplicationsDashboardView();
        return View("ApplicationDashboard", model);
    }
}