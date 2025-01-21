using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.DTOs;
using UserAPI.Services;

namespace UserAPI.Controllers;

[ApiController]
[Route("[controller]")]
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
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return user;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
    {
        try
        {
            var user = await userService.GetUserByUsernameAsync(username);
            return user;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserDtoCreate user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        try
        {
            var result = await userService.RegisterUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserDto user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        try
        {
            await userService.UpdateUserAsync(id, user);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(UserDtoLogin user)
    {
        try
        {
            var result = await userService.LoginUserAsync(user) ?? throw new Exception("User not found");
            return result;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}