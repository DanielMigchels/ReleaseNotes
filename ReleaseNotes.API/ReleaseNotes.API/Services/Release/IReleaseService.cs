using ReleaseNotes.API.Services.Release.Models;

namespace ReleaseNotes.API.Services.Release;

public interface IReleaseService
{
    public Task<List<ReleaseResponseModel>> GetRecentReleases();
    public Task<bool> PublishRelease(Guid releaseId);
    public Task<bool> UnpublishRelease(Guid releaseId);
    public Task<bool> CreateRelease(Guid projectId, CreateReleaseRequestModel createReleaseRequestModel);
    public Task<bool> EditRelease(Guid releaseId, EditReleaseRequestModel editReleaseRequestModel);
    public Task<bool> DeleteRelease(Guid releaseId);
}
