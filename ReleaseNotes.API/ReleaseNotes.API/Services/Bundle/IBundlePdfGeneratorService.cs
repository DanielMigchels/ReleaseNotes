using ReleaseNotes.API.Services.Bundle.Models;

namespace ReleaseNotes.API.Services.Bundle;

public interface IBundlePdfGeneratorService
{
    Stream GeneratePdf(BundleReleaseTimeRangesResponseModel releaseNotes);
}
