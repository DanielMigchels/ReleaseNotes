
using ReleaseNotes.API.Data.Models;
using ReleaseNotes.API.Services.Project.Models.ReleaseNotes;

namespace ReleaseNotes.API.Services.Bundle.Models;

public class BundleResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset CreatedOnUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public List<ReleaseNotesResponseModel> Projects { get; set; } = [];
    public List<BundleReleaseTimeRangesResponseModel> Releases { get; set; } = [];
}
