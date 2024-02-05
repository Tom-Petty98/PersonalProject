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
    public async Task<IActionResult> EditStatus(string refNumber)
    {
        var installer = await _getInstallerService.GetInstallerByReferenceNumberAsync(refNumber);
        var installerStatuses = await _getInstallerService.GetAllInstallerStatusesAsync();
        if (installer == null || installerStatuses == null)
        {
            return NotFound();
        }
        var model = new EditInstallerStatusViewModel()
        {
            InstallerStatuses = installerStatuses,
            RefNumber = installer.RefNumber,
            StatusId = installer.StatusId,
            ReviewRecommendation = installer.ReviewRecommendation,
            FlaggedForAudit = installer.FlaggedForAudit,
            LastEditedBy = installer.LastUpdatedBy ?? installer.CreatedBy,
            LastEditedDate = installer.LastUpdatedDate ?? installer.CreatedDate,
            InstallerDetail = installer.InstallerDetail
        };
        return View("EditStatus", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditStatus(EditInstallerStatusViewModel model)
    {
        if (ModelState.IsValid)
        {
            var installer = await _getInstallerService.GetInstallerByReferenceNumberAsync(model.RefNumber);
            if (installer == null)
            {
                return NotFound();
            }
            installer.Status = null;
            installer.StatusId = model.StatusId;
            installer.ReviewRecommendation = model.ReviewRecommendation;
            installer.FlaggedForAudit = model.FlaggedForAudit;
            installer.LastUpdatedBy = "Unknown";
            installer.LastUpdatedDate = DateTime.UtcNow;
            await _updateInstallerService.UpdateInstaller(installer);
        }
        return RedirectToAction(nameof(EditStatus), new { refNumber = model.RefNumber});
    }

    [HttpGet]
    public async Task<IActionResult> EditDetails(string refNumber)
    {
        var installer = await _getInstallerService.GetInstallerByReferenceNumberAsync(refNumber);
        if (installer == null)
        {
            return NotFound();
        }
        var installerDetail = installer.InstallerDetail;
        var model = new EditInstallerDetailsViewModel()
        {
            InstallerStatusDescription = installer.Status!.Description,
            RefNumber = installer.RefNumber,
            InstallerName = installerDetail.InstallerName,
            CompanyNumber = installerDetail.CompanyNumber,
            Postcode = installerDetail.InstallerAddress!.Postcode,
            UPRN = installerDetail.InstallerAddress!.UPRN,
            AddressLine1 = installerDetail.InstallerAddress.AddressLine1,
            AddressLine2 = installerDetail.InstallerAddress.AddressLine2,
            AddressLine3 = installerDetail.InstallerAddress.AddressLine3,
            LastEditedBy = installerDetail.LastUpdatedBy ?? installerDetail.CreatedBy,
            LastEditedDate = installerDetail.LastUpdatedDate ?? installerDetail.CreatedDate            
        };
        return View("EditDetails", model);
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
            return RedirectToAction(nameof(EditStatus), new { refNumber = model.RefNumber });
        }
        return View();
    }
}
