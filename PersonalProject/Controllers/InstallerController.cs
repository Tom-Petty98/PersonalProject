using Microsoft.AspNetCore.Mvc;
using PersonalProject.Domain.Constants;
using PersonalProject.Domain.Entities;
using PersonalProject.InternalPortal.Models.Installers;
using PersonalProject.InternalPortal.Services.Installers;
using static PersonalProject.Domain.Enums.InstallerStatus;

namespace PersonalProject.InternalPortal.Controllers;

public class InstallerController : Controller
{
    private readonly ILogger<InstallerController> _logger;
    private readonly IGetInstallerService _getInstallerService;
    private readonly IUpdateInstallerService _updateInstallerService;

    public InstallerController(ILogger<InstallerController> logger,
        IGetInstallerService getInstallerService,
        IUpdateInstallerService updateInstallerService)
    {
        _logger = logger;
        _getInstallerService = getInstallerService;
        _updateInstallerService = updateInstallerService;
    }

    public async Task<IActionResult> Index(string refNumber)
    {
        var model = await _getInstallerService.GetInstallerByReferenceNumberAsync(refNumber);
        return View("Installers/Details", model);
    }

    [HttpGet]    
    public IActionResult Create()
    {
        var model = new CreateInstallerViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInstallerViewModel model)
    {

        if (ModelState.IsValid)      
        {
            var installer = new Installer
            {
                StatusId = StatusMappings.InstallerStatuses[InstallerStatusCode.SUB],
                FlaggedForAudit = model.FlaggedForAudit,
                CreatedBy = "Unknown",
                CreatedDate = DateTime.UtcNow,
                InstallerDetail = new InstallerDetail
                {
                    InstallerName = model.InstallerName,
                    CompanyNumber = model.CompanyNumber,
                    CreatedBy = "Unknown",
                    CreatedDate = DateTime.UtcNow,
                    InstallerAddress = new Address
                    {
                        Postcode = model.Postcode,
                        UPRN = model.UPRN,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        AddressLine3 = model.AddressLine3,
                    }
                }
            };

            await _updateInstallerService.AddInstaller(installer);
            return RedirectToAction("Index", "Home");
        }   
        return View();
    }
}
