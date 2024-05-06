using Microsoft.AspNetCore.Mvc;
using PersonalProject.CoreAPI.Services.Installers;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;

namespace PersonalProject.CoreAPI.Controllers.Installers;

[ApiController]
[Route("Installers")]
public class InstallersController : ControllerBase
{
    private readonly ILogger<InstallersController> _logger;
    private readonly IGetInstallersService _getInstallersService;
    private readonly IUpdateInstallerService _updateInstallerService;

    public InstallersController(ILogger<InstallersController> logger,
        IGetInstallersService getInstallersService,
        IUpdateInstallerService updateInstallerService)
    {
        _logger = logger;
        _getInstallersService = getInstallersService;
        _updateInstallerService = updateInstallerService;
    }

    [HttpPost]
    [Route("AddInstaller")]
    public async Task<IActionResult> AddInstaller([FromBody] Installer installer)
    {
        _logger.LogInformation($"Adding new installer {installer.InstallerDetail.InstallerName}");
        
        try
        {
            var newApp = await _updateInstallerService.AddInstaller(installer);
            _logger.LogInformation($"Sucessfully created installer {installer.RefNumber}");
            return Ok(newApp);
        }
        catch (Exception ex)
        {
            const string message = "Unable to add installer";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpPost]
    [Route("UpdateInstaller")]
    public async Task<IActionResult> UpdateInstaller([FromBody] Installer installer)
    {
        _logger.LogInformation($"Updating Installer {installer.RefNumber}");

        try
        {
            var updatedApp = await _updateInstallerService.UpdateInstaller(installer);
            _logger.LogInformation($"Sucessfully updated installer {installer.RefNumber}");
            return Ok(updatedApp);
        }
        catch (Exception ex)
        {
            const string message = "Unable to update installer";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpPost]
    [Route("UpdateInstallerDetail")]
    public async Task<IActionResult> UpdateInstallerDetail([FromBody] InstallerDetail installerDetail)
    {
        _logger.LogInformation($"Updating installer detail {installerDetail.Id}");

        try
        {
            var updatedApp = await _updateInstallerService.UpdateInstallerDetail(installerDetail);
            _logger.LogInformation($"Sucessfully updated installer detail {installerDetail.Id}");
            return Ok(updatedApp);
        }
        catch (Exception ex)
        {
            const string message = "Unable to update installer";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetInstallerByReferenceNumber/{refNumber}")]
    public async Task<IActionResult> GetInstallerByReferenceNumber(string refNumber)
    {
        var installer = await _getInstallersService.GetInstallerByReferenceNumberAsync(refNumber);

        if (installer == null) 
            return NotFound($"No Installer found for ref number {refNumber}");

        return Ok(installer);
    }

    [HttpGet]
    [Route("GetInstallerById/{installerId}")]
    public async Task<IActionResult> GetInstallerById(int installerId)
    {
        var installer = await _getInstallersService.GetInstallerByIdAsync(installerId);

        if (installer == null)
            return NotFound($"No Installer found for id {installerId}");

        return Ok(installer);
    }

    [HttpGet]
    [Route("GetInstallerNameById/{installerId}")]
    public async Task<IActionResult> GetInstallerNameById(int installerId)
    {
        var installer = await _getInstallersService.GetInstallerNameByIdAsync(installerId);

        if (installer == null)
            return NotFound($"No Installer found for id {installerId}");

        return Ok(installer);
    }

    [HttpGet]
    [Route("GetAllInstallersDashboardView")]
    public async Task<IActionResult> GetAllInstallersDashboardView()
    {
        var installerDashboard = await _getInstallersService.GetAllInstallersDashboardView();
        return Ok(installerDashboard);
    }

    [HttpGet]
    [Route("GetPagedInstallers")]
    public async Task<IActionResult> GetPagedInstallers([FromBody] DashboardFilter dashboardFilter)
    {
        return Ok(await _getInstallersService.GetPagedInstallers(dashboardFilter));
    }

    [HttpGet]
    [Route("GetAllInstallerStatuses")]
    public async Task<IActionResult> GetAllInstallerStatuses()
    {
        return Ok(await _getInstallersService.GetAllInstallerStatusesAsync());
    }
}
