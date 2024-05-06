using Microsoft.AspNetCore.Mvc;
using PersonalProject.Domain.Constants;
using PersonalProject.Domain.Entities;
using PersonalProject.InternalPortal.Models.Applications;
using PersonalProject.InternalPortal.Services.Applications;
using PersonalProject.InternalPortal.Services.Implementation;
using PersonalProject.InternalPortal.Services.Installers;
using static PersonalProject.Domain.Enums.ApplicationStatus;

namespace PersonalProject.InternalPortal.Controllers;

public class ApplicationController : Controller
{
    private readonly ILogger<ApplicationController> _logger;
    private readonly IGetApplicationsService _getApplicationsService;
    private readonly IUpdateApplicationsService _updateApplicationsService;
    private readonly IGetInstallerService _getInstallerService;

    public ApplicationController(ILogger<ApplicationController> logger,
        IGetApplicationsService getApplicationsService,
        IUpdateApplicationsService updateApplicationsService,
        IGetInstallerService getInstallerService)
    {
        _logger = logger;
        _getApplicationsService = getApplicationsService;
        _updateApplicationsService = updateApplicationsService;
        _getInstallerService = getInstallerService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int userId, int installerId)
    {
        var installerName = await _getInstallerService.GetInstallerNameByIdAsync(installerId);
        if(installerName == null)
            return NotFound();

        var model = new CreateApplicationViewModel()
        {
            UserId = userId,
            InstallerId = installerId,
            InstallerName = installerName,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateApplicationViewModel model)
    {

        if (ModelState.IsValid)
        {
            var application = new Application
            {
                StatusId = StatusMappings.ApplicationStatuses[AppStatusCode.SUB],
                FlaggedForAudit = model.FlaggedForAudit,
                InstallerId =model.InstallerId,
                CurrentContactId = model.UserId,
                CreatedBy = "Unknown",
                CreatedDate = DateTime.UtcNow,
                ApplicationDetail = new ApplicationDetail
                {
                    SubmittedDate = model.SubmittedDate,
                    PropertyOwnerEmail = model.PropertyOwnerEmail,
                    CreatedBy = "Unknown",
                    CreatedDate = DateTime.UtcNow,
                    InstallationAddress = new Address
                    {
                        Postcode = model.Postcode,
                        UPRN = model.UPRN,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        AddressLine3 = model.AddressLine3,
                    }
                }
            };

            await _updateApplicationsService.AddApplication(application);
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> EditStatus(string refNumber)
    {
        var application = await _getApplicationsService.GetApplicationByReferenceNumberAsync(refNumber);
        var applicationStatuses = await _getApplicationsService.GetAllApplicationStatusesAsync();
        if (application == null || applicationStatuses == null)
        {
            return NotFound();
        }
        var installerName = await _getInstallerService.GetInstallerNameByIdAsync(application.InstallerId);

        var model = new EditApplicationStatusViewModel()
        {
            ApplicationStatuses = applicationStatuses,
            RefNumber = application.RefNumber,
            ApplicationId = application.Id,
            StatusId = application.StatusId,
            ReviewRecommendation = application.ReviewRecommendation,
            FlaggedForAudit = application.FlaggedForAudit,
            LastEditedBy = application.LastUpdatedBy ?? application.CreatedBy,
            LastEditedDate = application.LastUpdatedDate ?? application.CreatedDate,
            ApplicationDetail = application.ApplicationDetail,
            CurrentContact = application.CurrentContact!,
            InstallerName = installerName,
        };
        return View("EditStatus", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditStatus(EditApplicationStatusViewModel model)
    {
        if (ModelState.IsValid)
        {
            var application = await _getApplicationsService.GetApplicationByReferenceNumberAsync(model.RefNumber);
            if (application == null)
            {
                return NotFound();
            }
            application.Status = null;
            application.StatusId = model.StatusId;
            application.ReviewRecommendation = model.ReviewRecommendation;
            application.FlaggedForAudit = model.FlaggedForAudit;
            application.LastUpdatedBy = "Unknown";
            application.LastUpdatedDate = DateTime.UtcNow;
            await _updateApplicationsService.UpdateApplication(application);
        }
        return RedirectToAction(nameof(EditStatus), new { refNumber = model.RefNumber });
    }

    [HttpGet]
    public async Task<IActionResult> EditDetails(string refNumber)
    {
        var application = await _getApplicationsService.GetApplicationByReferenceNumberAsync(refNumber);
        if (application == null)
        {
            return NotFound();
        }
        var installerName = await _getInstallerService.GetInstallerNameByIdAsync(application.InstallerId);
        var applicationDetail = application.ApplicationDetail;
        var model = new EditApplicationDetailsViewModel()
        {
            ApplicationStatusDescription = application.Status!.Description,
            RefNumber = application.RefNumber,
            InstallerName = installerName,
            SubmittedDate = applicationDetail.SubmittedDate,
            PropertyOwnerEmail = applicationDetail.PropertyOwnerEmail,
            Postcode = applicationDetail.InstallationAddress!.Postcode,
            UPRN = applicationDetail.InstallationAddress!.UPRN,
            AddressLine1 = applicationDetail.InstallationAddress.AddressLine1,
            AddressLine2 = applicationDetail.InstallationAddress.AddressLine2,
            AddressLine3 = applicationDetail.InstallationAddress.AddressLine3,
            LastEditedBy = applicationDetail.LastUpdatedBy ?? applicationDetail.CreatedBy,
            LastEditedDate = applicationDetail.LastUpdatedDate ?? applicationDetail.CreatedDate
        };
        return View("EditDetails", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditDetails(EditApplicationDetailsViewModel model)
    {
        if (ModelState.IsValid)
        {
            var application = await _getApplicationsService.GetApplicationByReferenceNumberAsync(model.RefNumber);
            if (application == null)
            {
                return NotFound();
            }
            var applicationDetail = application.ApplicationDetail;

            applicationDetail.SubmittedDate = model.SubmittedDate;
            applicationDetail.PropertyOwnerEmail = model.PropertyOwnerEmail;
            applicationDetail.InstallationAddress!.Postcode = model.Postcode;
            applicationDetail.InstallationAddress.UPRN = model.UPRN;
            applicationDetail.InstallationAddress.AddressLine1 = model.AddressLine1;
            applicationDetail.InstallationAddress.AddressLine2 = model.AddressLine2;
            applicationDetail.InstallationAddress.AddressLine3 = model.AddressLine3;
            applicationDetail.LastUpdatedBy = "Unknown";
            applicationDetail.LastUpdatedDate = DateTime.UtcNow;
            await _updateApplicationsService.UpdateApplicationDetail(applicationDetail);
            return RedirectToAction(nameof(EditStatus), new { refNumber = model.RefNumber });
        }
        return View();
    }
}