using ReleaseNotes.API.Data;
using ReleaseNotes.API.Data.Models;
using ReleaseNotes.API.Services.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ReleaseNotes.API.Services.Users;

public class UserService(DatabaseContext db, UserManager<User> userManager) : IUserService
{

    public async Task<List<UserResponseModel>> GetUsers(string currentUserId)
    {
        var users = await db.Users.Select(x => new UserResponseModel()
        {
            Id = x.Id,
            Email = x.Email,
            Activated = x.HasAccess,
            IsCurrentUser = x.Id == currentUserId
        }).OrderBy(x => x.Email).ToListAsync();

        return users;
    }

    public async Task<bool> ChangeAccess(string currentUserId, string userId, ChangeActivationRequestModel model)
    {
        if (currentUserId == userId)
        {
            return false;
        }

        var user = await db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

        if (user == null)
        {
            return false;
        }

        user.HasAccess = model.Activated;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CreateUser(CreateUserRequestModel model)
    {
        var userExists = await userManager.FindByEmailAsync(model.Email);

        if (userExists != null)
        {
            return false;
        }

        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            HasAccess = true,
        };

        await userManager.CreateAsync(user, model.Password);
        return true;
    }

    public async Task<bool> UpdatePassword(string userId, UpdatePasswordRequestModel model)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        return result.Succeeded;
    }
}
