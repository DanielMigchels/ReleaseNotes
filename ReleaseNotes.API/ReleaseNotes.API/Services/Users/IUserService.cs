using ReleaseNotes.API.Services.Users.Models;

namespace ReleaseNotes.API.Services.Users;

public interface IUserService
{
    public Task<List<UserResponseModel>> GetUsers(string currentUserId);
    public Task<bool> ChangeAccess(string currentUserId, string userId, ChangeActivationRequestModel model);
    public Task<bool> CreateUser(CreateUserRequestModel model);
    public Task<bool> UpdatePassword(string userId, UpdatePasswordRequestModel model);
}
