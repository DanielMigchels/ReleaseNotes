namespace ReleaseNotes.API.Services.Authentication.Models;

public class LoginResponseModel
{
    public string Jwt { get; set; } = string.Empty;
    public bool Success { get; set; }
}
