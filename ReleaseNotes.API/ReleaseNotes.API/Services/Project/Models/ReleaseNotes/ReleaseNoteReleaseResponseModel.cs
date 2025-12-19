using ReleaseNotes.API.Enums;

namespace ReleaseNotes.API.Services.Project.Models.ReleaseNotes;

public class ReleaseNoteReleaseResponseModel
{
    public Guid Id { get; set; }
    public string Version { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? PublishedOn { get; set; }
    public IEnumerable<ReleaseNotesReleaseNoteEntryResponseModel> NoteEntries { get; set; } = [];
}
