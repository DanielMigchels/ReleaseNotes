namespace ReleaseNotes.API.Services.Release.Models;

public class ReleaseResponseModel
{
    public Guid Id { get; set; }
    public string Version { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public Guid? ProjectId { get; set; }
}
