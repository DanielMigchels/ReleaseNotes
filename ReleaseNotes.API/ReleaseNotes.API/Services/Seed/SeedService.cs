using ReleaseNotes.API.Data;
using ReleaseNotes.API.Services.Authentication;
using ReleaseNotes.API.Services.Authentication.Models;
using ReleaseNotes.API.Data.Models;
using ReleaseNotes.API.Enums;
using Microsoft.EntityFrameworkCore;

namespace ReleaseNotes.API.Services.Seed;

public class SeedService(DatabaseContext db, IAuthenticationService authenticationService) : ISeedService
{
    private readonly Random random = new();

    public async Task SeedFakeData()
    {
        if (!db.Users.Any())
        {
            await authenticationService.Register(new RegisterRequestModel
            {
                Email = "admin@example.com",
                Password = "password",
                HasAccess = true,
            });
        }

        var adminUserId = db.Users.First().Id;

        if (!db.Projects.Any())
        {
            var projects = new List<Data.Models.Project>
            {
                new() { Name = "Sample App Redesign", CreatedOnUtc = DateTimeOffset.UtcNow, CreatedByUserId = adminUserId },
                new() { Name = "Internal CRM System", CreatedOnUtc = DateTimeOffset.UtcNow, CreatedByUserId = adminUserId },
                new() { Name = "Website Revamp", CreatedOnUtc = DateTimeOffset.UtcNow, CreatedByUserId = adminUserId },
                new() { Name = "Data Analytics Platform", CreatedOnUtc = DateTimeOffset.UtcNow, CreatedByUserId = adminUserId }
            };

            foreach (var project in projects)
            {
                project.Releases = CreateReleases(project, random.Next(4, 14));
            }

            db.Projects.AddRange(projects);
            await db.SaveChangesAsync();
        }
    }

    private List<Data.Models.Release> CreateReleases(Data.Models.Project project, int numberOfReleases)
    {
        var releases = new List<Data.Models.Release>();
        var majorVersion = 1;
        var minorVersion = 0;
        var currentDate = DateTimeOffset.UtcNow;

        for (int i = numberOfReleases; 0 <= i; i--)
        {
            var version = $"{majorVersion}.{minorVersion}.0";
            DateTimeOffset? published = currentDate.AddDays(-i * 7).AddMinutes(random.Next(0, 60));

            if (i == 0)
            {
                published = null;
            }

            releases.Add(CreateRelease(project, version, published));

            if (minorVersion < 2)
            {
                minorVersion++;
            }
            else
            {
                majorVersion++;
                minorVersion = 0;
            }
        }

        return releases;
    }

    private Data.Models.Release CreateRelease(Data.Models.Project project, string version, DateTimeOffset? publishedOn)
    {
        var release = new Data.Models.Release
        {
            Version = version,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            Project = project,
            PublishedOn = publishedOn,
        };

        var notes = CreateIssues();
        foreach (var note in notes)
        {
            note.Release = release;
            release.Notes.Add(note);
        }

        return release;
    }

    private List<NoteEntry> CreateIssues()
    {
        var count = random.Next(3, 15);
        var issues = new List<NoteEntry>();
        var issueTitles = new (string text, NoteEntryType type)[]
        {
            // Bugfixes
            ("Fix login issue on mobile devices", NoteEntryType.Bugfix),
            ("Resolve data sync problem on backend", NoteEntryType.Bugfix),
            ("Address UI bug in settings panel", NoteEntryType.Bugfix),
            ("Fix crash on password reset page", NoteEntryType.Bugfix),
            ("Fix memory leak in report generation module", NoteEntryType.Bugfix),
            ("Resolve issue with multiple file downloads", NoteEntryType.Bugfix),
            ("Fix search bar filtering not working on certain conditions", NoteEntryType.Bugfix),
            ("Fix user profile image upload not working in Safari", NoteEntryType.Bugfix),
            ("Resolve issue with incorrect time zone conversions", NoteEntryType.Bugfix),
            ("Fix bug causing duplicate email notifications", NoteEntryType.Bugfix),
            ("Fix problem with notifications not being marked as read", NoteEntryType.Bugfix),
            ("Fix 404 errors when navigating to old URLs", NoteEntryType.Bugfix),
            ("Fix issue with drag-and-drop functionality on dashboard", NoteEntryType.Bugfix),
            ("Fix problem with pagination on user management screen", NoteEntryType.Bugfix),
            ("Fix broken links in the footer section", NoteEntryType.Bugfix),
            ("Fix issue with dark mode not applying to all components", NoteEntryType.Bugfix),
            ("Resolve bug causing users to be logged out unexpectedly", NoteEntryType.Bugfix),
            ("Fix bug where notifications are displayed multiple times", NoteEntryType.Bugfix),
            ("Resolve issue with modal dialogs not closing properly", NoteEntryType.Bugfix),
            ("Fix alignment issue on form labels in mobile view", NoteEntryType.Bugfix),
            ("Fix issue with duplicate entries being saved in the database", NoteEntryType.Bugfix),
            ("Fix issue with incorrect permissions in role management", NoteEntryType.Bugfix),
            ("Fix styling issues on Internet Explorer", NoteEntryType.Bugfix),
            ("Fix bug where autocomplete suggestions are not shown", NoteEntryType.Bugfix),
            ("Fix issue where session cookies expire too early", NoteEntryType.Bugfix),
            ("Fix bug causing incorrect totals in reports", NoteEntryType.Bugfix),

            // New Features
            ("Improve loading speed on dashboard", NoteEntryType.NewFeature),
            ("Add validation to ensure mandatory fields are filled", NoteEntryType.NewFeature),
            ("Add logging for failed API requests", NoteEntryType.NewFeature),
            ("Improve accessibility for screen reader users", NoteEntryType.NewFeature),
            ("Improve error handling for file uploads", NoteEntryType.NewFeature),
            ("Add feature to export user data to CSV", NoteEntryType.NewFeature),
            ("Improve security by adding rate limiting to login page", NoteEntryType.NewFeature),
            ("Improve responsiveness of the navbar on smaller screens", NoteEntryType.NewFeature),
            ("Improve session persistence on browser refresh", NoteEntryType.NewFeature),
            ("Improve password strength validation", NoteEntryType.NewFeature),

            // Critical
            ("Address security vulnerability in token-based authentication", NoteEntryType.Critical),
            ("Optimize database queries for better performance", NoteEntryType.Critical),
            ("Resolve issue with PDF generation timing out", NoteEntryType.Critical),
            ("Fix issue with scrolling inside modals on mobile", NoteEntryType.Critical),
            ("Fix crash when uploading large files", NoteEntryType.Critical),
            ("Improve security headers for API responses", NoteEntryType.Critical),
            ("Optimize background job processing performance", NoteEntryType.Critical),
            ("Resolve problem with PDF generation timing out", NoteEntryType.Critical),

            // Removal
            ("Remove deprecated payment gateway integration", NoteEntryType.Removal),
            ("Deprecate old admin panel functionality", NoteEntryType.Removal),
            ("Remove obsolete API endpoints", NoteEntryType.Removal),
            ("Remove legacy support for Internet Explorer", NoteEntryType.Removal),
            ("Remove outdated notification system", NoteEntryType.Removal),
        };


        for (int i = 0; i < count; i++)
        {
            var (text, type) = issueTitles[random.Next(0, issueTitles.Length)];

            issues.Add(new NoteEntry
            {
                Text = text,
                CreatedOnUtc = DateTimeOffset.UtcNow,
                Type = type
            });
        }

        return issues;
    }

    public async Task SeedMassiveFakeData()
    {
        if (!db.Users.Any())
        {
            await authenticationService.Register(new RegisterRequestModel
            {
                Email = "admin@example.com",
                Password = "password"
            });
        }

        var adminUserId = db.Users.First().Id;

        if (!db.Projects.Any())
        {
            var tasks = new List<Task>();

            for (int i = 0; i < 1000000; i++)
            {
                var project = new Data.Models.Project
                {
                    Name = $"Project {i}",
                    CreatedOnUtc = DateTimeOffset.UtcNow,
                    CreatedByUserId = adminUserId,
                    LatestActivity = DateTimeOffset.UtcNow,
                };

                project.Releases = CreateReleases(project, random.Next(4, 14));

                db.Projects.Add(project);

                if (i % 1000 == 0)
                {
                    tasks.Add(db.SaveChangesAsync());
                    var connectionString = db.Database.GetConnectionString();
                    db = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(connectionString).Options);
                }

                Console.WriteLine($"Project {i} seeded.");
            }

            await Task.WhenAll(tasks);
        }
    }
}