namespace ReleaseNotes.API.Services.Users.Models;

public class UserResponseModel
{
    public string? Email { get;  set; }
    public string Id { get; set; } = string.Empty;
    public bool Activated { get; set; } = false;
    public bool IsCurrentUser { get; set;} = false;
}
