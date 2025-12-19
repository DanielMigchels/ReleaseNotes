using ReleaseNotes.API.Services.Release;
using ReleaseNotes.API.Services.Release.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReleaseController(IReleaseService releaseService) : ReleaseNotesControllerBase
{
    [Authorize]
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentReleases()
    {
        var result = await releaseService.GetRecentReleases();
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{releaseId}/publish")]
    public async Task<IActionResult> PublishRelease([FromRoute] Guid releaseId)
    {
        var result = await releaseService.PublishRelease(releaseId);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPost("{releaseId}/unpublish")]
    public async Task<IActionResult> UnpublishRelease([FromRoute] Guid releaseId)
    {
        var result = await releaseService.UnpublishRelease(releaseId);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPost("{projectId}")]
    public async Task<IActionResult> CreateRelease([FromRoute] Guid projectId, CreateReleaseRequestModel createReleaseRequestModel)
    {
        var result = await releaseService.CreateRelease(projectId, createReleaseRequestModel);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPut("{releaseId}")]
    public async Task<IActionResult> EditRelease([FromRoute] Guid releaseId, EditReleaseRequestModel editReleaseRequestModel)
    {
        var result = await releaseService.EditRelease(releaseId, editReleaseRequestModel);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpDelete("{releaseId}")]
    public async Task<IActionResult> DeleteRelease([FromRoute] Guid releaseId)
    {
        var result = await releaseService.DeleteRelease(releaseId);
        return result ? Ok() : BadRequest();
    }
}
