
using ReleaseNotes.API.Services.Project.Models.ReleaseNotes;

namespace ReleaseNotes.API.Services.Bundle.Models;

public class BundleReleaseTimeRangesResponseModel
{
    public DateTimeOffset? CreatedOnUtc { get; set; }
    public string Version { get; set; } = string.Empty;
    public DateTimeOffset? StartTimeUtc { get; set; }
    public DateTimeOffset? EndTimeUtc { get; set; }
    public List<ReleaseNotesResponseModel> ReleaseNote { get; set; } = [];
    public Guid Id { get; internal set; }
    public string BundleName { get; internal set; } = string.Empty;
}
