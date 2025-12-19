using ReleaseNotes.API.Data;
using ReleaseNotes.API.Enums;
using ReleaseNotes.API.Services.Pagination;
using ReleaseNotes.API.Services.Project.Models;
using ReleaseNotes.API.Services.Project.Models.ReleaseNotes;
using Microsoft.EntityFrameworkCore;

namespace ReleaseNotes.API.Services.Project;

public class ProjectService(DatabaseContext db, IProjectPdfGeneratorService projectPdfGeneratorService) : IProjectService
{
    public async Task<PaginatedList<ProjectResponseModel>> GetProjects(int pageSize = 2147483647, int page = 0)
    {
        var data = await db.Projects.AsNoTracking()
            .OrderByDescending(x => x.LatestActivity ?? x.CreatedOnUtc)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(x => new ProjectResponseModel()
            {
                Id = x.Id,
                Name = x.Name,
                LatestVersion = x.Releases.Where(x => x.PublishedOn != null).OrderByDescending(x => x.CreatedOnUtc).Select(x => x.Version).FirstOrDefault() ?? string.Empty,
                CreatedBy = x.CreatedByUser!.Email!,
                CreatedOnUtc = x.CreatedOnUtc,
                LatestVersionCreatedOnUtc = x.Releases.Where(x => x.PublishedOn != null).OrderByDescending(x => x.CreatedOnUtc).Select(x => x.CreatedOnUtc).FirstOrDefault()
            })
            .ToListAsync();

        return new PaginatedList<ProjectResponseModel>()
        {
            Data = data,
            HasNext = await db.Projects.Skip((page + 1) * pageSize).AnyAsync(),
            HasPrevious = page > 0,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ReleaseNotesResponseModel?> GetReleaseNotes(Guid projectId)
    {
        var project = await db.Projects.AsNoTracking()
            .Where(x => x.Id == projectId)
            .Select(x => new ReleaseNotesResponseModel()
            {
                Id = x.Id,
                Name = x.Name,
                LatestVersion = x.Releases.Where(y => y.PublishedOn != null).OrderByDescending(y => y.CreatedOnUtc).Select(y => y.Version).FirstOrDefault() ?? string.Empty,
                CreatedBy = x.CreatedByUser!.Email!,
                CreatedOnUtc = x.CreatedOnUtc,
                Releases = x.Releases.OrderByDescending(x => x.CreatedOnUtc).Select(y => new ReleaseNoteReleaseResponseModel()
                {
                    Id = y.Id,
                    Version = y.Version,
                    CreatedOnUtc = y.CreatedOnUtc,
                    PublishedOn = y.PublishedOn,
                    NoteEntries = y.Notes.OrderBy(z => z.Type == null ? NoteEntryType.Bugfix : z.Type).ThenByDescending(z => z.CreatedOnUtc).Select(z => new ReleaseNotesReleaseNoteEntryResponseModel()
                    {
                        Id = z.Id,
                        Text = z.Text,
                        Url = z.Url,
                        Type = z.Type,
                        CreatedOnUtc = z.CreatedOnUtc,
                        ReleasePublishedOn = z.Release!.PublishedOn,
                    })
                })
            })
            .FirstOrDefaultAsync();

        return project;
    }

    public async Task<Stream?> DownloadPdf(Guid projectId, bool includeUnpublished = false)
    {
        var releaseNotes = await GetReleaseNotes(projectId);

        if (releaseNotes == null)
        {
            return null;
        }

        var pdf = projectPdfGeneratorService.GeneratePdf(releaseNotes, includeUnpublished);
        return pdf;
    }

    public async Task<AddProjectResponseModel> AddProjects(AddProjectRequestModel requestModel, string userId)
    {
        var project = new Data.Models.Project()
        {
            Id = Guid.NewGuid(),
            Name = requestModel.Name,
            CreatedByUserId = userId,
            CreatedOnUtc = DateTimeOffset.UtcNow,
        };

        db.Projects.Add(project);
        await db.SaveChangesAsync();
        
        return new AddProjectResponseModel()
        {
            Id = project.Id
        };
    }

    public async Task<bool> EditProject(Guid projectId, EditProjectRequestModel requestModel)
    {
        var project = await db.Projects.Where(x => x.Id == projectId).FirstOrDefaultAsync();
        if (project == null)
        {
            return false;
        }

        project.Name = requestModel.Name;
        project.LatestActivity = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProject(Guid projectId)
    {
        var project = await db.Projects.Include(x => x.Releases).ThenInclude(x => x.Notes).Where(x => x.Id == projectId).FirstOrDefaultAsync();
        if (project == null)
        {
            return false;
        }

        db.RemoveRange(project.Releases);
        db.Remove(project);
        await db.SaveChangesAsync();
        return true;
    }
}
