namespace ReleaseNotes.API.Services.Users.Models;

public class CreateUserRequestModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
