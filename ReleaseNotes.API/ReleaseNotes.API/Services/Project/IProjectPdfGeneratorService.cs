using ReleaseNotes.API.Services.Project.Models.ReleaseNotes;

namespace ReleaseNotes.API.Services.Project;

public interface IProjectPdfGeneratorService
{
    public Stream GeneratePdf(ReleaseNotesResponseModel releaseNotes, bool includeUnpublished = false);
}
