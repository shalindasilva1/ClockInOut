using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.DTOs;
using UserAPI.Services;

namespace UserAPI.Controllers;

public class UsersController(IUserService userService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        return await userService.GetUserByIdAsync(id) is { } user ? user : NotFound();
    }
    
    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
    {
        return await userService.GetUserByUsernameAsync(username) is { } user ? user : NotFound();
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        await userService.RegisterUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserDto user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        await userService.UpdateUserAsync(id, user);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await userService.DeleteUserAsync(id);
        return NoContent();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(UserDto user)
    {
        var result = await userService.GetUserByUsernameAsync(user.Username);
        if (result.PasswordHash == user.PasswordHash)
        {
            return Ok(result);
        }
        return Unauthorized();
    }
}