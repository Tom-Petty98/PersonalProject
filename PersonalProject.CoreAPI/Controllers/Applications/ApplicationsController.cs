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
    public IActionResult AddApplication([FromBody] Application application)
    {
        _logger.LogInformation($"Adding new application for installer {application.InstallerId}");
        
        try
        {
            var newApp = _updateApplicationService.AddApplication(application);
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
    public IActionResult UpdateApplication([FromBody] Application application)
    {
        _logger.LogInformation($"Updating application {application.RefNumber}");

        try
        {
            var updatedApp = _updateApplicationService.UpdateApplication(application);
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
    public IActionResult UpdateApplicationDetail([FromBody] ApplicationDetail applicationDetail)
    {
        _logger.LogInformation($"Updating application detail {applicationDetail.Id}");

        try
        {
            var updatedApp = _updateApplicationService.UpdateApplicationDetail(applicationDetail);
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
    public IActionResult GetApplicationByReferenceNumber(string refNumber)
    {
        var application = _getApplicationsService.GetApplicationByReferenceNumberAsync(refNumber);

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
    public IActionResult GetAllApplicationStatuses()
    {
        return Ok(_getApplicationsService.GetAllApplicationStatusesAsync());
    }
}
