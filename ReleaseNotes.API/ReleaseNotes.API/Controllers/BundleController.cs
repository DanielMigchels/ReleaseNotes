using ReleaseNotes.API.Services.Bundle;
using ReleaseNotes.API.Services.Bundle.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BundleController(IBundleService bundleService) : ReleaseNotesControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBundles([FromQuery] int pageSize = 2147483647, [FromQuery] int page = 0)
    {
        var result = await bundleService.GetBundles(pageSize, page);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{bundleId}")]
    public async Task<IActionResult> GetBundle([FromRoute] Guid bundleId)
    {
        var result = await bundleService.GetBundle(bundleId);
        return result != null ? Ok(result) : BadRequest();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddProject([FromBody] CreateBundleRequestModel requestModel)
    {
        var result = await bundleService.AddBundle(requestModel, UserId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{bundleId}")]
    public async Task<IActionResult> EditProject([FromRoute] Guid bundleId, [FromBody] EditBundleRequestModel requestModel)
    {
        var result = await bundleService.EditBundle(bundleId, requestModel);
        return result ? Ok(result) : BadRequest();
    }

    [Authorize]
    [HttpDelete("{bundleId}")]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid bundleId)
    {
        var result = await bundleService.DeleteBundle(bundleId);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPut("{bundleId}/projects")]
    public async Task<IActionResult> AddProjectsToBundle([FromRoute] Guid bundleId, [FromBody] AddProjectsToBundleRequestModel requestModel)
    {
        var result = await bundleService.AddProjectsToBundle(bundleId, requestModel);
        return result ? Ok(result) : BadRequest();
    }

    [Authorize]
    [HttpPost("{bundleId}/release")]
    public async Task<IActionResult> AddRelease([FromRoute] Guid bundleId, [FromBody] CreateReleaseBundleRequestModel requestModel)
    {
        var result = await bundleService.AddRelease(bundleId, requestModel);
        return result ? Ok(result) : BadRequest();
    }

    [Authorize]
    [HttpPut("release/{bundleReleaseId}")]
    public async Task<IActionResult> EditBundleRelease([FromRoute] Guid bundleReleaseId, [FromBody] CreateReleaseBundleRequestModel requestModel)
    {
        var result = await bundleService.EditBundleRelease(bundleReleaseId, requestModel);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpDelete("release/{bundleReleaseId}")]
    public async Task<IActionResult> DeleteBundleRelease([FromRoute] Guid bundleReleaseId)
    {
        var result = await bundleService.DeleteBundleRelease(bundleReleaseId);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpGet("release/{bundleReleaseId}/pdf")]
    public async Task<IActionResult> DownloadPdf([FromRoute] Guid bundleReleaseId)
    {
        var result = await bundleService.DownloadPdf(bundleReleaseId);
        return result != null ? File(result, "application/pdf", "ReleaseNotes.pdf") : BadRequest();
    }
}