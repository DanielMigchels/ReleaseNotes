using ReleaseNotes.API.Services.Bundle.Models;
using ReleaseNotes.API.Services.Pagination;

namespace ReleaseNotes.API.Services.Bundle;

public interface IBundleService
{
    Task<CreateBundleResponseModel> AddBundle(CreateBundleRequestModel requestModel, string userId);
    Task<bool> AddProjectsToBundle(Guid bundleId, AddProjectsToBundleRequestModel requestModel);
    Task<bool> AddRelease(Guid bundleId, CreateReleaseBundleRequestModel requestModel);
    Task<bool> DeleteBundle(Guid bundleId);
    Task<bool> DeleteBundleRelease(Guid bundleReleaseId);
    Task<Stream?> DownloadPdf(Guid bundleReleaseId);
    Task<bool> EditBundle(Guid bundleId, EditBundleRequestModel requestModel);
    Task<bool> EditBundleRelease(Guid bundleReleaseId, CreateReleaseBundleRequestModel requestModel);
    Task<BundleResponseModel?> GetBundle(Guid projectId);
    Task<PaginatedList<BundleResponseModel>> GetBundles(int pageSize, int page);
}
