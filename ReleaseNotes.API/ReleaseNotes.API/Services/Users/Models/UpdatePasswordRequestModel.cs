namespace ReleaseNotes.API.Services.Users.Models;

public class UpdatePasswordRequestModel
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string RepeatNewPassword { get; set; } = string.Empty;
}
