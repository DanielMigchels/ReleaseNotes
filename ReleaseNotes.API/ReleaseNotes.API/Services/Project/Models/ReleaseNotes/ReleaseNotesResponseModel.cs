namespace ReleaseNotes.API.Services.Project.Models.ReleaseNotes;

public class ReleaseNotesResponseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string LatestVersion { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public IEnumerable<ReleaseNoteReleaseResponseModel> Releases { get; set; } = [];
}
