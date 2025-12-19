using ReleaseNotes.API.Services.Note;
using ReleaseNotes.API.Services.Note.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteController(INoteService noteService) : ReleaseNotesControllerBase
{
    [Authorize]
    [HttpPost("{releaseId}")]
    public async Task<IActionResult> CreateNote([FromRoute] Guid releaseId, [FromBody] CreateNoteRequestModel createNoteRequestModel)
    {
        await noteService.CreateNote(releaseId, createNoteRequestModel);
        return Ok();
    }

    [Authorize]
    [HttpPut("{noteId}/{releaseId}")]
    public async Task<IActionResult> MoveNote([FromRoute] Guid noteId, [FromRoute] Guid releaseId)
    {
        var result = await noteService.MoveNote(noteId, releaseId);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPut("{noteId}")]
    public async Task<IActionResult> EditNote([FromRoute] Guid noteId, [FromBody] EditNoteRequestModel editNoteRequestModel)
    {
        var result = await noteService.EditNode(noteId, editNoteRequestModel);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpDelete("{noteId}")]
    public async Task<IActionResult> DeleteNote([FromRoute] Guid noteId)
    {
        var result = await noteService.DeleteNote(noteId);
        return result ? Ok() : BadRequest();
    }
}
