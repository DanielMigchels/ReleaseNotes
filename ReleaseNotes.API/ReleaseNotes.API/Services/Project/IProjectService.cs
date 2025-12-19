using ReleaseNotes.API.Services.Pagination;
using ReleaseNotes.API.Services.Project.Models;
using ReleaseNotes.API.Services.Project.Models.ReleaseNotes;

namespace ReleaseNotes.API.Services.Project;

public interface IProjectService
{
    public Task<PaginatedList<ProjectResponseModel>> GetProjects(int pageSize = 2147483647, int page = 0);
    public Task<ReleaseNotesResponseModel?> GetReleaseNotes(Guid projectId);
    public Task<Stream?> DownloadPdf(Guid projectId, bool includeUnpublished = false);
    public Task<AddProjectResponseModel> AddProjects(AddProjectRequestModel requestModel, string userId);
    public Task<bool> EditProject(Guid projectId, EditProjectRequestModel requestModel);
    public Task<bool> DeleteProject(Guid projectId);
}
