namespace ReleaseNotes.API.Data.Models;

public class BundleReleaseTimeRange
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Version { get; set; } = string.Empty;
    public DateTimeOffset? CreatedOnUtc { get; set; }
    public DateTimeOffset? StartTimeUtc { get; set; }
    public DateTimeOffset? EndTimeUtc { get; set; }
    public Guid BundleId { get; set; }
    public virtual Bundle? Bundle { get; set; }
}
