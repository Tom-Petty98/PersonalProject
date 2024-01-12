using Microsoft.AspNetCore.Mvc;
using PersonalProject.InternalPortal.Services.Interfaces;
using PersonalProject.Models;
using System.Diagnostics;

namespace PersonalProject.Controllers.Installers;

public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly IInstallerService _installerService;

    public DashboardController(ILogger<DashboardController> logger, IInstallerService installerService)
    {
        _logger = logger;
        _installerService = installerService;
    }

    public IActionResult Index()
    {
        _installerService.
        return View();
    }
}