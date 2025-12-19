using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ReleaseNotes.API.Controllers;

public class ReleaseNotesControllerBase : ControllerBase
{
    protected string UserId => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
}
