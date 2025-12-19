namespace ReleaseNotes.API.Services.Authentication.Models;

public class RegisterRequestModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool HasAccess { get; set; } = false;
}
