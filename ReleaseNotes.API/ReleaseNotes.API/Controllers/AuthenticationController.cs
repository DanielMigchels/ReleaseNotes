using ReleaseNotes.API.Services.Authentication;
using ReleaseNotes.API.Services.Authentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
    {
        var result = await authenticationService.Login(loginRequestModel);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Result([FromBody] RegisterRequestModel registerRequestModel)
    {
        var result = await authenticationService.Register(registerRequestModel);
        return Ok(result);
    }
}
