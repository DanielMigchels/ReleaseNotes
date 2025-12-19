using ReleaseNotes.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes.API.Data.Models;

public class NoteEntry
{
    public Guid Id { get; set; }
    [MaxLength(4096)]
    public string Text { get; set; } = string.Empty;
    [MaxLength(4096)]
    public string? Url { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public NoteEntryType? Type { get; set; }

    public Guid? ReleaseId { get; set; }
    public Release? Release { get; set; }

    public override string ToString()
    {
        return Text;
    }
}
