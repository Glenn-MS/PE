using Microsoft.AspNetCore.Mvc;
using PlatformOrchestrator.Api.Models;
using PlatformOrchestrator.Api.Services;

namespace PlatformOrchestrator.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    [Roles("admin")]
    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [Roles("admin")]
    [HttpPost("users")]
    public IActionResult AddUser([FromBody] User user)
    {
        _userService.AddUser(user);
        return Created($"api/admin/users/{user.Id}", user);
    }

    [Roles("admin")]
    [HttpPut("users/{userId}")]
    public IActionResult UpdateUser(int userId, [FromBody] User user)
    {
        _userService.UpdateUser(userId, user);
        return NoContent();
    }

    [Roles("admin")]
    [HttpDelete("users/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        _userService.DeleteUser(userId);
        return NoContent();
    }
}
