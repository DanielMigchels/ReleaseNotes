using ReleaseNotes.API.Enums;

namespace ReleaseNotes.API.Services.Project.Models.ReleaseNotes;

public class ReleaseNotesReleaseNoteEntryResponseModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string? Url { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public NoteEntryType? Type { get; set; }
    public DateTimeOffset? ReleasePublishedOn { get; set; }
}
