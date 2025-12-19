using ReleaseNotes.API.Enums;

namespace ReleaseNotes.API.Services.Note.Models;

public class EditNoteRequestModel
{
    public string Text { get; set; } = string.Empty;
    public string? Url { get; set; }
    public NoteEntryType? Type { get; set; }
}