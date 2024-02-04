using Microsoft.AspNetCore.Mvc;
using PersonalProject.Domain.Constants;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Enums;
using PersonalProject.InternalPortal.Models.Installers;
using PersonalProject.InternalPortal.Services.Installers;
using System.Security.Cryptography;
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

    [HttpGet]
    public async Task<IActionResult> EditDetails(string refNumber)
    {
        var installer = await _getInstallerService.GetInstallerByReferenceNumberAsync(refNumber);
        var installerStatuses = await _getInstallerService.GetAllInstallerStatusesAsync();
        if (installer == null || installerStatuses == null)
        {
            return NotFound();
        }
        var model = new EditInstallerViewModel()
        {
            InstallerStatuses = installerStatuses,
            Status = new EditInstallerStatusViewModel()
            {
                RefNumber = installer.RefNumber,
                StatusId = installer.StatusId,
                ReviewRecommendation = installer.ReviewRecommendation,
                FlaggedForAudit = installer.FlaggedForAudit,
                LastEditedBy = installer.LastUpdatedBy ?? installer.CreatedBy,
                LastEditedDate = installer.LastUpdatedDate ?? installer.CreatedDate
            },
            Detail = new EditInstallerDetailsViewModel()
            {
                RefNumber = installer.RefNumber,
                InstallerName = installer.InstallerDetail.InstallerName,
                CompanyNumber = installer.InstallerDetail.CompanyNumber,
                Postcode = installer.InstallerDetail.InstallerAddress!.Postcode,
                UPRN = installer.InstallerDetail.InstallerAddress!.UPRN,
                AddressLine1 = installer.InstallerDetail.InstallerAddress.AddressLine1,
                AddressLine2 = installer.InstallerDetail.InstallerAddress.AddressLine2,
                AddressLine3 = installer.InstallerDetail.InstallerAddress.AddressLine3,
                LastEditedBy = installer.InstallerDetail.LastUpdatedBy ?? installer.InstallerDetail.CreatedBy,
                LastEditedDate = installer.InstallerDetail.LastUpdatedDate ?? installer.InstallerDetail.CreatedDate
            }                   
        };
        return View("Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditStatus(EditInstallerStatusViewModel statusModel)
    {
        if (ModelState.IsValid)
        {
            var installer = await _getInstallerService.GetInstallerByReferenceNumberAsync(statusModel.RefNumber);
            if (installer == null)
            {
                return NotFound();
            }
            installer.StatusId = statusModel.StatusId;
            installer.ReviewRecommendation = statusModel.ReviewRecommendation;
            installer.FlaggedForAudit = statusModel.FlaggedForAudit;
            installer.LastUpdatedBy = "Unknown";
            installer.LastUpdatedDate = DateTime.UtcNow;
            await _updateInstallerService.UpdateInstaller(installer);
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EditDetails(EditInstallerDetailsViewModel model)
    {
        if (ModelState.IsValid)
        {
            var installer = await _getInstallerService.GetInstallerByReferenceNumberAsync(model.RefNumber);
            if (installer == null)
            {
                return NotFound();
            }
            var installerDetail = installer.InstallerDetail;

            installerDetail.InstallerName = model.InstallerName;
            installerDetail.CompanyNumber = model.CompanyNumber;
            installerDetail.InstallerAddress!.Postcode = model.Postcode;
            installerDetail.InstallerAddress.UPRN = model.UPRN;
            installerDetail.InstallerAddress.AddressLine1 = model.AddressLine1;
            installerDetail.InstallerAddress.AddressLine2 = model.AddressLine2;
            installerDetail.InstallerAddress.AddressLine3 = model.AddressLine3;
            installerDetail.LastUpdatedBy = "Unknown";
            installerDetail.LastUpdatedDate = DateTime.UtcNow;
            await _updateInstallerService.UpdateInstallerDetail(installerDetail);
            return RedirectToAction(nameof(Index));
        }
        return View();
    }
}
