
namespace ReleaseNotes.API.Services.Release.Models;

public class EditReleaseRequestModel
{
    public string Version { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
}
