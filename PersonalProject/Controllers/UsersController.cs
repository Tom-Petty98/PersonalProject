using Microsoft.AspNetCore.Mvc;
using PersonalProject.Domain.Entities;
using PersonalProject.InternalPortal.Models.Users;
using PersonalProject.InternalPortal.Services.Installers;

namespace PersonalProject.InternalPortal.Controllers;

public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;
    private readonly IGetInstallerService _getInstallerService;
    private readonly IGetUsersService _getUsersService;
    private readonly IUpdateUsersService _updateUsersService;

    public UsersController(ILogger<UsersController> logger,
        IGetInstallerService getInstallerService,
        IGetUsersService getUsersService,
        IUpdateUsersService updateUsersService)
    {
        _logger = logger;
        _getInstallerService = getInstallerService;
        _getUsersService = getUsersService;
        _updateUsersService = updateUsersService;
    }

    [HttpGet]
    public async Task<IActionResult> Details(int installerId)
    {
        var installer = await _getInstallerService.GetInstallerByIdAsync(installerId);
        if (installer == null) return NotFound();
        var users = await _getUsersService.GetUsersByInstallerIdAsync(installerId);
        var model = new UserDetailsViewModel()
        {
            InstallerId = installerId,
            InstallerName = installer.InstallerDetail.InstallerName,
            RefNumber = installer.RefNumber,
            Users = users!
        };

        return View("Details", model);
    }

    [HttpGet]
    public IActionResult Create(int installerId)
    {
        var model = new CreateUserViewModel();
        model.InstallerId = installerId;
        return View(model);
    }

    [HttpGet]
    public IActionResult CreateAuthRep(int installerId)
    {
        var model = new CreateUserViewModel();
        model.InstallerId = installerId;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExternalUser(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userRoles = await _getUsersService.GetUserRolesAsync(false);
            var userRole = userRoles.First(x => x.Id == model.RoleId);
            var user = new User
            {
                Email = model.Email,
                InstallerId = model.InstallerId,
                Roles = new List<Role> { userRole },
                CreatedBy = "Unknown",
                CreatedDate = DateTime.UtcNow
            };

            await _updateUsersService.AddUser(user);
            return RedirectToAction(nameof(Details), new { installerId = user.InstallerId });
        }
        return View();
    }

    [HttpGet]
    public async Task <IActionResult> Edit(int userId)
    {
        var user = await _getUsersService.GetUserByIdAsync(userId);

        if (user == null) return NotFound();
        
        var model = new EditUserViewModel()
        {
            Email = user.Email,
            RoleId = user.Roles.First().Id,
            UserId = user.Id,
            InstallerId = user.InstallerId
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _getUsersService.GetUserByIdAsync(model.UserId);
            var roles = await _getUsersService.GetUserRolesAsync(false);
            var role = roles.First(x => x.Id == model.RoleId);

            if (user == null) return NotFound();
            user.Email = model.Email;
            user.Roles = new List<Role> { role };
            await _updateUsersService.UpdateUser(user);
            return RedirectToAction(nameof(Details), new { installerId = user.InstallerId });
        }       

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int userId)
    {
        var user = await _getUsersService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        user.IsObselete = true;
        user.LastUpdatedDate = DateTime.UtcNow;
        user.LastUpdatedBy = "Unknown";
        await _updateUsersService.UpdateUser(user);

        return RedirectToAction(nameof(Details), new { installerId = user.InstallerId });
    }

    [HttpPost]
    public async Task<IActionResult> Reactivate(int userId)
    {
        var user = await _getUsersService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        user.IsObselete = false;
        user.LastUpdatedDate = DateTime.UtcNow;
        user.LastUpdatedBy = "Unknown";
        await _updateUsersService.UpdateUser(user);

        return RedirectToAction(nameof(Details), new { installerId = user.InstallerId });
    }
}
