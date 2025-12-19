using ReleaseNotes.API.Data;
using ReleaseNotes.API.Data.Models;
using ReleaseNotes.API.Services.Note.Models;
using Microsoft.EntityFrameworkCore;

namespace ReleaseNotes.API.Services.Note;

public class NoteService(DatabaseContext db) : INoteService
{
    public async Task CreateNote(Guid releaseId, CreateNoteRequestModel createNoteRequestModel)
    {
        var noteEntry = new NoteEntry
        {
            Text = createNoteRequestModel.Text,
            Url = createNoteRequestModel.Url,
            Type = createNoteRequestModel.Type,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            ReleaseId = releaseId,
        };

        db.NoteEntries.Add(noteEntry);

        var project = await db.Projects.Where(x => x.Releases.Any(y => y.Id == releaseId)).FirstOrDefaultAsync();
        if (project != null)
        {
            project.LatestActivity = DateTimeOffset.UtcNow;
        }

        await db.SaveChangesAsync();
    }

    public async Task<bool> MoveNote(Guid noteId, Guid releaseId)
    {
        var note = await db.NoteEntries.Include(x => x.Release).Where(x => x.Id == noteId).FirstOrDefaultAsync();
        if (note == null)
        {
            return false;
        }

        if (note.Release?.PublishedOn != null)
        {
            return false;
        }

        var targetRelease = await db.Releases.Include(x => x.Project).Where(x => x.Id == releaseId).FirstOrDefaultAsync();
        if (targetRelease == null)
        {
            return false;
        }
        if (targetRelease.PublishedOn != null)
        {
            return false;
        }

        note.ReleaseId = releaseId;

        targetRelease.Project!.LatestActivity = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EditNode(Guid noteId, EditNoteRequestModel editNoteRequestModel)
    {
        var note = await db.NoteEntries.Include(x => x.Release).ThenInclude(x => x!.Project).Where(x => x.Id == noteId).FirstOrDefaultAsync();
        if (note == null)
        {
            return false;
        }

        note.Text = editNoteRequestModel.Text;
        note.Url = editNoteRequestModel.Url;
        note.Type = editNoteRequestModel.Type;
        note.Release!.Project!.LatestActivity = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteNote(Guid noteId)
    {
        var note = await db.NoteEntries.Include(x => x.Release).ThenInclude(x => x!.Project).Where(x => x.Id == noteId).FirstOrDefaultAsync();
        if (note == null)
        {
            return false;
        }
        
        db.Remove(note);
        note.Release!.Project!.LatestActivity = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }
}
