namespace ReleaseNotes.API.Services.Project.Models;

public class ProjectResponseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string LatestVersion { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset LatestVersionCreatedOnUtc { get; set; }
}
