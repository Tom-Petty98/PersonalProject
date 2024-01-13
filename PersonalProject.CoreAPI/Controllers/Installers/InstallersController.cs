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
            _logger.LogInformation($"Sucessfully created Installer {installer.RefNumber}");
            return Ok(newApp);
        }
        catch (Exception ex)
        {
            const string message = "Unable to add Installer";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpPost]
    [Route("UpdateInstaller")]
    public async Task <IActionResult> UpdateInstaller([FromBody] Installer installer)
    {
        _logger.LogInformation($"Updating Installer {installer.RefNumber}");

        try
        {
            var updatedApp = await _updateInstallerService.AddInstaller(installer);
            _logger.LogInformation($"Sucessfully updated onstaller {installer.RefNumber}");
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
        var Installer = await _getInstallersService.GetInstallerByReferenceNumberAsync(refNumber);

        if (Installer == null) 
            return NotFound($"No Installer found for ref number {refNumber}");

        return Ok(Installer);
    }

    [HttpGet]
    [Route("GetAllInstallersDashboardView")]
    public async Task<IActionResult> GetAllInstallersDashboardView()
    {
        return Ok(await _getInstallersService.GetAllInstallersDashboardView());
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