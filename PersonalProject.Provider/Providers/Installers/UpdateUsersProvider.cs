using Microsoft.EntityFrameworkCore;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using System.Collections.Generic;

namespace PersonalProject.Provider.Providers.Installers;

public interface IUpdateUsersProvider
{
    Task<int> AddUser(User user);
    Task<bool> UpdateUser(User user);
}

public class UpdateUsersProvider : IUpdateUsersProvider
{
    private readonly ApplicationDbContext _context;
    private readonly IGetUsersProvider _getUsersProvider;

    public UpdateUsersProvider(ApplicationDbContext context, IGetUsersProvider getUsersProvider)
    {
        _context = context;
        _getUsersProvider = getUsersProvider;
    }

    public async Task<int> AddUser(User user)
    {
        var roles = _context.Roles.ToList();
        List<Role> selectedRoles = new List<Role>();
        foreach (var role in user.Roles)
        {
            selectedRoles.Add(roles.First(x => x.Id == role.Id));
        }

        user.Roles = selectedRoles;
        var newUserId = _context.Users.Add(user).Entity.Id;
        await _context.SaveChangesAsync();
        return newUserId;
    }

    public async Task<bool> UpdateUser(User user)
    {
        var foundUser = await _context.Users.Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        if (foundUser == null) 
            throw new BadRequestException("User not found", System.Net.HttpStatusCode.NotFound);


        if (user.Email != foundUser.Email)
        {
            bool isDuplicate = await _getUsersProvider.IsEmailInUse(user.Email);
            if (isDuplicate) 
                throw new ArgumentException("Email address aready in use.");
        }

        _context.Entry(foundUser).CurrentValues.SetValues(user);
        int result = await _context.SaveChangesAsync();

        var rolesMatchingOrginal = user.Roles.IntersectBy(foundUser.Roles.Select(x => x.Id), x => x.Id);
        if (rolesMatchingOrginal.Count() != user.Roles.Count)
        {
            await UpdateUserRoles(user.Id, user.Roles);
        }
        
        return result > 0;
    }

    private async Task UpdateUserRoles(int userId, ICollection<Role> newRoles)
    {
        var user = _context.Users.First(x => x.Id == userId);

        var dbRoles = _context.Roles.ToList();
        List<Role> selectedRoles = [];
        foreach (var role in newRoles)
        {
            selectedRoles.Add(dbRoles.First(x => x.Id == role.Id));
        }

        user.Roles = selectedRoles;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
