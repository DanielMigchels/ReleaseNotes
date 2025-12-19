namespace ReleaseNotes.API.Data.Models;

public class Release
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Version { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? PublishedOn { get; set; }

    public Guid? ProjectId { get; set; }
    public virtual Project? Project { get; set; }

    public virtual ICollection<NoteEntry> Notes { get; set; } = [];
}
