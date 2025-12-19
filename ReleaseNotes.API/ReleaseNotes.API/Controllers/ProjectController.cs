using ReleaseNotes.API.Services.Project;
using ReleaseNotes.API.Services.Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController(IProjectService projectService) : ReleaseNotesControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetProjects([FromQuery] int pageSize = 2147483647, [FromQuery] int page = 0)
    {
        var result = await projectService.GetProjects(pageSize, page);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetProjects([FromRoute] Guid projectId)
    {
        var result = await projectService.GetReleaseNotes(projectId);
        return result != null ? Ok(result) : BadRequest();
    }

    [Authorize]
    [HttpGet("{projectId}/pdf")]
    public async Task<IActionResult> DownloadPdf([FromRoute] Guid projectId, [FromQuery] bool includeUnpublished = false)
    {
        var result = await projectService.DownloadPdf(projectId, includeUnpublished);
        return result != null ? File(result, "application/pdf", "ReleaseNotes.pdf") : BadRequest();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddProject([FromBody] AddProjectRequestModel requestModel)
    {
        var result = await projectService.AddProjects(requestModel, UserId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{projectId}")]
    public async Task<IActionResult> EditProject([FromRoute] Guid projectId, [FromBody] EditProjectRequestModel requestModel)
    {
        var result = await projectService.EditProject(projectId, requestModel);
        return result ? Ok(result) : BadRequest();
    }

    [Authorize]
    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid projectId)
    {
        var result = await projectService.DeleteProject(projectId);
        return result ? Ok() : BadRequest();
    }
}