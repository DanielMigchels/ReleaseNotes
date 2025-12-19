using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes.API.Data.Models;

public class Bundle
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    public DateTimeOffset CreatedOnUtc { get; set; }

    public string? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public ICollection<BundleReleaseTimeRange> BundleReleaseTimeRanges { get; set; } = [];
    public ICollection<BundleProject> BundleProjects { get; set; } = [];
}
