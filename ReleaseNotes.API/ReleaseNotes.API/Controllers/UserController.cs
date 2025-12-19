using ReleaseNotes.API.Services.Users;
using ReleaseNotes.API.Services.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService userService) : ReleaseNotesControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.GetUsers(UserId);
        return Ok(users);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel model)
    {
        var result = await userService.CreateUser(model);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestModel model)
    {
        var result = await userService.UpdatePassword(UserId, model);
        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPut("{userId}")]
    public async Task<IActionResult> ChangeAccess([FromRoute] string userId, [FromBody] ChangeActivationRequestModel model)
    {
        var result = await userService.ChangeAccess(UserId, userId, model);
        return result ? Ok() : BadRequest();
    }
}
