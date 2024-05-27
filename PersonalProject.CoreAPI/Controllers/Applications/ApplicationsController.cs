using Microsoft.AspNetCore.Mvc;
using PersonalProject.CoreAPI.Services.Applications;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;

namespace PersonalProject.CoreAPI.Controllers.Applications;

[ApiController]
[Route("Applications")]
public class ApplicationsController : ControllerBase
{
    private readonly ILogger<ApplicationsController> _logger;
    private readonly IGetApplicationsService _getApplicationsService;
    private readonly IUpdateApplicationService _updateApplicationService;

    public ApplicationsController(ILogger<ApplicationsController> logger,
        IGetApplicationsService getApplicationsService,
        IUpdateApplicationService updateApplicationService)
    {
        _logger = logger;
        _getApplicationsService = getApplicationsService;
        _updateApplicationService = updateApplicationService;
    }

    [HttpPost]
    [Route("AddApplication")]
    public async Task<IActionResult> AddApplication([FromBody] Application application)
    {
        _logger.LogInformation($"Adding new application for installer {application.InstallerId}");
        
        try
        {
            var newApp = await _updateApplicationService.AddApplication(application);
            _logger.LogInformation($"Sucessfully created application {application.RefNumber}");
            return Ok(newApp);
        }
        catch (Exception ex)
        {
            const string message = "Unable to add application";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpPost]
    [Route("UpdateApplication")]
    public async Task<IActionResult> UpdateApplication([FromBody] Application application)
    {
        _logger.LogInformation($"Updating application {application.RefNumber}");

        try
        {
            var updatedApp = await _updateApplicationService.UpdateApplication(application);
            _logger.LogInformation($"Sucessfully updated application {application.RefNumber}");
            return Ok(updatedApp);
        }
        catch (Exception ex)
        {
            const string message = "Unable to update application";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpPost]
    [Route("UpdateApplicationDetail")]
    public async Task<IActionResult> UpdateApplicationDetail([FromBody] ApplicationDetail applicationDetail)
    {
        _logger.LogInformation($"Updating application detail {applicationDetail.Id}");

        try
        {
            var updatedApp = await _updateApplicationService.UpdateApplicationDetail(applicationDetail);
            _logger.LogInformation($"Sucessfully updated application detail {applicationDetail.Id}");
            return Ok(updatedApp);
        }
        catch (Exception ex)
        {
            const string message = "Unable to update application";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetApplicationByReferenceNumber/{refNumber}")]
    public async Task<IActionResult> GetApplicationByReferenceNumber(string refNumber)
    {
        var application = await _getApplicationsService.GetApplicationByReferenceNumberAsync(refNumber);

        if (application == null) 
            return NotFound($"No application found for ref number {refNumber}");

        return Ok(application);
    }

    [HttpGet]
    [Route("GetAllApplicationsDashboardView")]
    public async Task<IActionResult> GetAllApplicationsDashboardView()
    {
        return Ok(await _getApplicationsService.GetAllApplicationsDashboardView());
    }

    [HttpGet]
    [Route("GetPagedApplications")]
    public async Task<IActionResult> GetPagedApplications([FromBody] DashboardFilter dashboardFilter)
    {
        return Ok(await _getApplicationsService.GetPagedApplications(dashboardFilter));
    }

    [HttpGet]
    [Route("GetAllApplicationStatuses")]
    public async Task<IActionResult> GetAllApplicationStatuses()
    {
        return Ok(await _getApplicationsService.GetAllApplicationStatusesAsync());
    }

    [HttpGet]
    [Route("GetTechTypes")]
    public async Task<IActionResult> GetTechTypes()
    {
        return Ok(await _getApplicationsService.GetTechTypesAsync());
    }
}
