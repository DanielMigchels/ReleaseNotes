using ReleaseNotes.API.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ReleaseNotes.API.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Bundle> Bundles { get; set; }
    public DbSet<BundleProject> BundleProjects { get; set; }
    public DbSet<BundleReleaseTimeRange> BundleReleaseTimeRanges { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Release> Releases { get; set; }
    public DbSet<NoteEntry> NoteEntries { get; set; }
}
