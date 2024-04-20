using Microsoft.AspNetCore.Mvc;
using PersonalProject.CoreAPI.Services.Installers;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;

namespace PersonalProject.CoreAPI.Controllers.Installers;

[ApiController]
[Route("Users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IGetUsersService _getUsersService;
    private readonly IUpdateUsersService _updateUsersService;

    public UsersController(ILogger<UsersController> logger,
        IGetUsersService getInstallersService,
        IUpdateUsersService updateInstallerService)
    {
        _logger = logger;
        _getUsersService = getInstallersService;
        _updateUsersService = updateInstallerService;
    }

    [HttpPost]
    [Route("AddUser")]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        _logger.LogInformation($"Adding new user {user.Email}");

        try
        {
            var newUser = await _updateUsersService.AddUserAsync(user);
            _logger.LogInformation($"Adding new user  {user.Email}");
            return Ok(newUser);
        }
        catch (Exception ex)
        {
            const string message = "Unable to add user";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpPost]
    [Route("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        _logger.LogInformation($"Adding new user {user.Email}");

        try
        {
            var updatedUser = await _updateUsersService.UpdateUserAsync(user);
            _logger.LogInformation($"Adding new user {user.Email}");
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            const string message = "Unable to update user";
            _logger.LogError(ex, message);
            return BadRequest($"{message} - {ex.Message}");
        }
    }

    [HttpGet]
    [Route("GetUsersByInstallerId/{installerId}")]
    public async Task<IActionResult> GetUserByInstallerId(int installerId)
    {
        var users = await _getUsersService.GetUsersByInstallerIdAsync(installerId);
        return Ok(users);
    }

    [HttpGet]
    [Route("GetUserById/{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var user = await _getUsersService.GetUserByIdAsync(userId);

        if (user == null)
            return NotFound($"No user found for id {userId}");

        return Ok(user);
    }


    [HttpGet]
    [Route("GetUserRoles/{getInternal}")]
    public async Task<IActionResult> GetUserRoles(bool getInternal)
    {
        return Ok(await _getUsersService.GetUserRolesAsync(getInternal));
    }
}
