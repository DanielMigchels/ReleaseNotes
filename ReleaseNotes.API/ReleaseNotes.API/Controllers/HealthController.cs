using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController(ILogger<HealthController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        logger.LogInformation("Health check performed.");
        return Ok("Healthy");
    }
}
