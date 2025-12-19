using ReleaseNotes.API.Services.Note.Models;

namespace ReleaseNotes.API.Services.Note;

public interface INoteService
{
    public Task CreateNote(Guid releaseId, CreateNoteRequestModel createNoteRequestModel);
    public Task<bool> MoveNote(Guid noteId, Guid releaseId);
    public Task<bool> EditNode(Guid noteId, EditNoteRequestModel editNoteRequestModel);
    public Task<bool> DeleteNote(Guid noteId);
}
