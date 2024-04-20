using PersonalProject.Domain.Entities;
using PersonalProject.Provider.Providers.Installers;

namespace PersonalProject.CoreAPI.Services.Installers;

public interface IUpdateUsersService
{
    Task<int> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
}
public class UpdateUsersService : IUpdateUsersService
{
    private readonly IUpdateUsersProvider _updateUsersProvider;
    private readonly IGetUsersProvider _getUsersProvider;

    public UpdateUsersService(IUpdateUsersProvider updateUsersProvider, IGetUsersProvider getUsersProvider)
    {
        _updateUsersProvider = updateUsersProvider;
        _getUsersProvider = getUsersProvider;
    }

    public async Task<int> AddUserAsync(User user)
    {
        bool isDuplicate = await _getUsersProvider.IsEmailInUse(user.Email);

        if (isDuplicate) 
            throw new ArgumentException("Email address aready in use.");

        return await _updateUsersProvider.AddUser(user);
    }

    public async Task<bool> UpdateUserAsync(User user)
        => await _updateUsersProvider.UpdateUser(user);   
}
