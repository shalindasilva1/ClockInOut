using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.DTOs;
using UserAPI.Services;

namespace UserAPI.Controllers;

/// <summary>
/// UsersController handles user-related operations such as retrieving, creating, updating, and deleting users.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService) : Controller
{
    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of users.</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID.</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>The user with the specified username.</returns>
    [Authorize]
    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
    {
        try
        {
            var user = await userService.GetUserByUsernameAsync(username);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The user data to create.</param>
    /// <returns>The created user.</returns>
    [Authorize]
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

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="user">The updated user data.</param>
    /// <returns>No content if the update is successful.</returns>
    [Authorize]
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

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [Authorize]
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

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="user">The user login data.</param>
    /// <returns>A JWT token if the login is successful.</returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login(UserDtoLogin user)
    {
        try
        {
            var result = await userService.LoginUserAsync(user) ?? throw new Exception("User not found");
            return Ok(new { token = result });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}