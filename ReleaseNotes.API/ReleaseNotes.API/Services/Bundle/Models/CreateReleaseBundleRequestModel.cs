
namespace ReleaseNotes.API.Services.Bundle.Models
{
    public class CreateReleaseBundleRequestModel
    {
        public string Version { get; set; } = string.Empty;
        public string? StartTimeUtc { get; set; } = string.Empty;
        public string? EndTimeUtc { get; set; } = string.Empty;
    }
}
