using Microsoft.AspNetCore.Mvc;
using PersonalProject.Domain.Request;
using PersonalProject.InternalPortal.Models.Home;
using PersonalProject.InternalPortal.Models.Installers;
using PersonalProject.InternalPortal.Services.Applications;
using PersonalProject.InternalPortal.Services.Installers;

namespace PersonalProject.InternalPortal.Controllers;

public class HomeController : Controller
{
    private readonly IGetInstallerService _getInstallerService;
    private readonly IGetApplicationsService _getApplicationsService;

    public HomeController(IGetInstallerService getInstallerService,
        IGetApplicationsService getApplicationsService)
    {
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
        //var dashboardFilter = get value from session.
        var model = new AppDashboardViewModel();
        model.ApplicationStatuses = await _getApplicationsService.GetAllApplicationStatusesAsync();
        model.Applications = await _getApplicationsService.GetPagedApplications(new DashboardFilter());
        return View("ApplicationDashboard", model);
    }

    [HttpPost]
    public IActionResult SearchBy(AppDashboardViewModel appDashboard)
    {
        if(appDashboard.SearchBy.Length < 3)
        {
            ModelState.AddModelError(nameof(appDashboard.SearchBy), "Your search must have 3 or more characters");
        }
        return RedirectToAction(nameof(ApplicationDashboard));
    }
}