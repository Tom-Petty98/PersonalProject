using PersonalProject.Domain.Entities;
using PersonalProject.Provider.Providers.Installers;

namespace PersonalProject.CoreAPI.Services.Installers;

public interface IGetUsersService
{
    Task<List<User>?> GetUsersByInstallerIdAsync(int installerId);
    Task<User?> GetUserByIdAsync(int userId);
    Task<IEnumerable<Role>> GetUserRolesAsync(bool getInternal);
}


public class GetUsersService : IGetUsersService
{
    private readonly IGetUsersProvider _getUsersProvider;

    public GetUsersService(IGetUsersProvider usersProvider)
    {
        _getUsersProvider = usersProvider;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
        => await _getUsersProvider.GetUserById(userId);

    public async Task<IEnumerable<Role>> GetUserRolesAsync(bool getInternal)
        => await _getUsersProvider.GetUserRolesAsync(getInternal);

    public async Task<List<User>?> GetUsersByInstallerIdAsync(int installerId)
        => await _getUsersProvider.GetUsersByInstallerId(installerId);
}

