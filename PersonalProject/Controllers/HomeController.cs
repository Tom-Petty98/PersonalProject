using Microsoft.AspNetCore.Mvc;
using PersonalProject.Domain.Request;
using PersonalProject.InternalPortal.Models.Home;
using PersonalProject.InternalPortal.Services.Applications;
using PersonalProject.InternalPortal.Services.Helpers;
using PersonalProject.InternalPortal.Services.Installers;

namespace PersonalProject.InternalPortal.Controllers;

public class HomeController : Controller
{
    private readonly IGetInstallerService _getInstallerService;
    private readonly IGetApplicationsService _getApplicationsService;
    private DashboardFilter _dashboardFilters = new();
    private const string AppDashFilterKey = "AppDashFilterKey";

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
        var sessionDashFilters = HttpContext.Session.GetOrDefault<DashboardFilter>(AppDashFilterKey);
        if (sessionDashFilters != null)
        {
            _dashboardFilters = sessionDashFilters;
        }

        var model = new AppDashboardViewModel();
        model.ApplicationStatuses = await _getApplicationsService.GetAllApplicationStatusesAsync();
        model.Applications = await _getApplicationsService.GetPagedApplications(_dashboardFilters);
        return View("ApplicationDashboard", model);
    }

    [HttpPost]
    public IActionResult ApplyFilters(AppDashboardViewModel appDashboard)
    {
        _dashboardFilters.FilteredAppStatuses = appDashboard.StatusCodesToFilterBy;
        HttpContext.Session.Put(AppDashFilterKey, _dashboardFilters);
        return RedirectToAction(nameof(ApplicationDashboard));
    }

    [HttpPost]
    public IActionResult ClearFilters()
    {
        HttpContext.Session.Put(AppDashFilterKey, new DashboardFilter());
        return RedirectToAction(nameof(ApplicationDashboard));
    }

    [HttpPost]
    public IActionResult SearchBy(AppDashboardViewModel appDashboard)
    {
        if (appDashboard.SearchBy == null || appDashboard.SearchBy.Length < 3)
        {
            ModelState.AddModelError("SearchBy", "Your search must have 3 or more characters");
        }
        else
        {
            _dashboardFilters.SearchBy = appDashboard.SearchBy;
            HttpContext.Session.Put(AppDashFilterKey, _dashboardFilters);
        }
        return RedirectToAction(nameof(ApplicationDashboard));
    }

    [HttpPost]
    public IActionResult SortBy(string column)
    {
        var sessionDashFilters = HttpContext.Session.GetOrDefault<DashboardFilter>(AppDashFilterKey) ?? new();

        if(string.CompareOrdinal(sessionDashFilters.SortBy, column) == 0)
            sessionDashFilters.OrderByDescending = !sessionDashFilters.OrderByDescending;
        else
            sessionDashFilters.OrderByDescending = true;


        sessionDashFilters.SortBy = column;
        HttpContext.Session.Put(AppDashFilterKey, sessionDashFilters);
        return RedirectToAction(nameof(ApplicationDashboard));
    }

    [HttpGet]
    public IActionResult ChangePage(int selectedPage)
    {
        _dashboardFilters.PageNum = selectedPage;
        HttpContext.Session.Put(AppDashFilterKey, _dashboardFilters);
        return RedirectToAction(nameof(ApplicationDashboard));
    }
}