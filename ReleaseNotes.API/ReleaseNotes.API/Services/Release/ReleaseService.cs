using ReleaseNotes.API.Data;
using ReleaseNotes.API.Services.Release.Models;
using Microsoft.EntityFrameworkCore;

namespace ReleaseNotes.API.Services.Release;

public class ReleaseService(DatabaseContext db) : IReleaseService
{
    public async Task<List<ReleaseResponseModel>> GetRecentReleases()
    {
        return await db.Releases.AsNoTracking()
            .Where(x => x.PublishedOn != null)
            .OrderByDescending(r => r.PublishedOn)
            .Take(5)
            .Select(x => new ReleaseResponseModel
            {
                Id = x.Id,
                Version = x.Version,
                ProjectName = x.Project!.Name,
                ProjectId = x.ProjectId,
            })
            .ToListAsync();
    }

    public async Task<bool> PublishRelease(Guid releaseId)
    {
        var release = await db.Releases.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == releaseId);

        if (release == null)
        {
            return false;
        }

        release.PublishedOn = DateTimeOffset.UtcNow;
        release.Project!.LatestActivity = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnpublishRelease(Guid releaseId)
    {
        var release = await db.Releases.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == releaseId);

        if (release == null)
        {
            return false;
        }

        release.PublishedOn = null;
        release.Project!.LatestActivity = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CreateRelease(Guid projectId, CreateReleaseRequestModel createReleaseRequestModel)
    {
        var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == projectId);
        if (project == null)
        {
            return false;
        }

        project.LatestActivity = DateTimeOffset.UtcNow;

        var release = new Data.Models.Release()
        {
            ProjectId = projectId,
            Version = createReleaseRequestModel.Version,
            CreatedOnUtc = DateTimeOffset.UtcNow,
        };

        db.Releases.Add(release);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EditRelease(Guid releaseId, EditReleaseRequestModel editReleaseRequestModel)
    {
        var release = await db.Releases.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == releaseId);
        if (release == null)
        {
            return false;
        }
        release.Project!.LatestActivity = DateTimeOffset.UtcNow;
        release.CreatedOnUtc = editReleaseRequestModel.CreatedOnUtc.ToUniversalTime();
        release.Version = editReleaseRequestModel.Version;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRelease(Guid releaseId)
    {
        var release = await db.Releases.Include(x => x.Project).Include(x => x.Notes).FirstOrDefaultAsync(x => x.Id == releaseId);
        if (release == null)
        {
            return false;
        }
        release.Project!.LatestActivity = DateTimeOffset.UtcNow;
        db.RemoveRange(release.Notes);
        db.Remove(release);
        await db.SaveChangesAsync();
        return true;
    }
}
