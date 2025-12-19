using ReleaseNotes.API.Data;
using ReleaseNotes.API.Enums;
using ReleaseNotes.API.Services.Bundle.Models;
using ReleaseNotes.API.Services.Pagination;
using ReleaseNotes.API.Services.Project.Models.ReleaseNotes;
using Microsoft.EntityFrameworkCore;

namespace ReleaseNotes.API.Services.Bundle;

public class BundleService(DatabaseContext db, IBundlePdfGeneratorService bundlePdfGeneratorService) : IBundleService
{
    public async Task<PaginatedList<BundleResponseModel>> GetBundles(int pageSize, int page)
    {
        var data = await db.Bundles.AsNoTracking()
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(x => new BundleResponseModel()
            {
                Id = x.Id,
                Name = x.Name,
                CreatedOnUtc = x.CreatedOnUtc,
                CreatedBy = x.CreatedByUser!.Email!,
            })
            .ToListAsync();

        return new PaginatedList<BundleResponseModel>()
        {
            Data = data,
            HasNext = await db.Projects.Skip((page + 1) * pageSize).AnyAsync(),
            HasPrevious = page > 0,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<BundleResponseModel?> GetBundle(Guid bundleId)
    {
        var bundle = await db.Bundles.AsNoTracking()
            .Where(x => x.Id == bundleId)
            .Select(x => new BundleResponseModel()
            {
                Id = x.Id,
                Name = x.Name,
                CreatedOnUtc = x.CreatedOnUtc,
                CreatedBy = x.CreatedByUser!.Email!,
                Projects = x.BundleProjects.Select(x => new ReleaseNotesResponseModel()
                {
                    Id = x.Project!.Id,
                    Name = x.Project!.Name,
                    CreatedBy = x.Project.CreatedByUser!.Email!,
                    CreatedOnUtc = x.Project.CreatedOnUtc,
                }).ToList(),
                Releases = x.BundleReleaseTimeRanges.Select(brtr => new BundleReleaseTimeRangesResponseModel()
                {
                    Id = brtr.Id,
                    BundleName = x.Name,
                    Version = brtr.Version,
                    StartTimeUtc = brtr.StartTimeUtc,
                    EndTimeUtc = brtr.EndTimeUtc,
                    CreatedOnUtc = brtr.CreatedOnUtc,
                    ReleaseNote = brtr.Bundle!.BundleProjects.Select(p => new ReleaseNotesResponseModel()
                    {
                        Id = p.Project!.Id,
                        Name = p.Project!.Name,
                        CreatedBy = p.Project.CreatedByUser!.Email!,
                        CreatedOnUtc = p.Project.CreatedOnUtc,
                        Releases = p.Project.Releases.Where(r => r.PublishedOn != null && (brtr.StartTimeUtc == null || r.CreatedOnUtc >= brtr.StartTimeUtc) && (brtr.EndTimeUtc == null || r.CreatedOnUtc <= brtr.EndTimeUtc))
                        .Select(y => new ReleaseNoteReleaseResponseModel()
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
                        }).OrderByDescending(r => r.CreatedOnUtc).ToList()
                    }).ToList()
                })
                .OrderByDescending(x => x.CreatedOnUtc).ToList()
            })
            .FirstOrDefaultAsync();

        return bundle;
    }

    public async Task<CreateBundleResponseModel> AddBundle(CreateBundleRequestModel requestModel, string userId)
    {
        var bundle = new Data.Models.Bundle()
        {
            Id = Guid.NewGuid(),
            Name = requestModel.Name,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            CreatedByUserId = userId,
        };

        db.Bundles.Add(bundle);
        await db.SaveChangesAsync();

        return new CreateBundleResponseModel()
        {
            Id = bundle.Id
        };
    }

    public async Task<bool> EditBundle(Guid bundleId, EditBundleRequestModel requestModel)
    {
        var bundle = await db.Bundles.Where(x => x.Id == bundleId).FirstOrDefaultAsync();
        if (bundle == null)
        {
            return false;
        }

        bundle.Name = requestModel.Name;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBundle(Guid bundleId)
    {
        var bundle = await db.Bundles.Where(x => x.Id == bundleId).FirstOrDefaultAsync();
        if (bundle == null)
        {
            return false;
        }

        db.Remove(bundle);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddProjectsToBundle(Guid bundleId, AddProjectsToBundleRequestModel requestModel)
    {
        var bundle = await db.Bundles.Where(x => x.Id == bundleId).FirstOrDefaultAsync();
        if (bundle == null)
        {
            return false;
        }

        var bundleProjects = db.BundleProjects.Where(x => x.BundleId == bundleId).ToList();
        db.RemoveRange(bundleProjects);

        foreach (var projectId in requestModel.Projects)
        {
            db.BundleProjects.Add(new Data.Models.BundleProject()
            {
                Bundle = bundle,
                ProjectId = projectId
            });
        }

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddRelease(Guid bundleId, CreateReleaseBundleRequestModel requestModel)
    {
        var bundle = await db.Bundles.Where(x => x.Id == bundleId).FirstOrDefaultAsync();
        if (bundle == null)
        {
            return false;
        }

        DateTime? startTimeUtc = DateTime.TryParse(requestModel.StartTimeUtc, out var s) ? DateTime.SpecifyKind(s, DateTimeKind.Utc) : null;
        DateTime? endTimeUtc = DateTime.TryParse(requestModel.EndTimeUtc, out var e) ? DateTime.SpecifyKind(e, DateTimeKind.Utc) : null;

        if (endTimeUtc.HasValue)
        {
            endTimeUtc = endTimeUtc.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        db.BundleReleaseTimeRanges.Add(new()
        {
            Bundle = bundle,
            BundleId = bundle.Id,
            Version = requestModel.Version,
            StartTimeUtc = startTimeUtc,
            EndTimeUtc = endTimeUtc,
            CreatedOnUtc = DateTimeOffset.UtcNow
        });

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EditBundleRelease(Guid bundleReleaseId, CreateReleaseBundleRequestModel requestModel)
    {
        var bundle = await db.BundleReleaseTimeRanges.Where(x => x.Id == bundleReleaseId).FirstOrDefaultAsync();
        if (bundle == null)
        {
            return false;
        }

        DateTime? startTimeUtc = DateTime.TryParse(requestModel.StartTimeUtc, out var s) ? DateTime.SpecifyKind(s, DateTimeKind.Utc) : null;
        DateTime? endTimeUtc = DateTime.TryParse(requestModel.EndTimeUtc, out var e) ? DateTime.SpecifyKind(e, DateTimeKind.Utc) : null;

        if (endTimeUtc.HasValue)
        {
            endTimeUtc = endTimeUtc.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        bundle.Version = requestModel.Version;
        bundle.StartTimeUtc = startTimeUtc;
        bundle.EndTimeUtc = endTimeUtc;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBundleRelease(Guid bundleReleaseId)
    {
        var bundle = await db.BundleReleaseTimeRanges.Where(x => x.Id == bundleReleaseId).FirstOrDefaultAsync();
        if (bundle == null)
        {
            return false;
        }

        db.Remove(bundle);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<Stream?> DownloadPdf(Guid bundleReleaseId)
    {
        var bundle = await db.BundleReleaseTimeRanges.Where(x => x.Id == bundleReleaseId).Select(brtr => new BundleReleaseTimeRangesResponseModel()
        {
            Id = brtr.Id,
            BundleName = brtr.Bundle!.Name,
            Version = brtr.Version,
            StartTimeUtc = brtr.StartTimeUtc,
            EndTimeUtc = brtr.EndTimeUtc,
            CreatedOnUtc = brtr.CreatedOnUtc,
            ReleaseNote = brtr.Bundle!.BundleProjects.Select(p => new ReleaseNotesResponseModel()
            {
                Id = p.Project!.Id,
                Name = p.Project!.Name,
                CreatedBy = p.Project.CreatedByUser!.Email!,
                CreatedOnUtc = p.Project.CreatedOnUtc,
                Releases = p.Project.Releases.Where(r => r.PublishedOn != null && (brtr.StartTimeUtc == null || r.CreatedOnUtc >= brtr.StartTimeUtc) && (brtr.EndTimeUtc == null || r.CreatedOnUtc <= brtr.EndTimeUtc))
                .Select(y => new ReleaseNoteReleaseResponseModel()
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
                }).OrderByDescending(r => r.CreatedOnUtc).ToList()
            }).ToList()
        }).FirstOrDefaultAsync();

        if (bundle == null)
        {
            return null;
        }

        var pdf = bundlePdfGeneratorService.GeneratePdf(bundle);
        return pdf;
    }
}
