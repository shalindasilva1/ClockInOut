using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using TeamAPI.Models.DTOs;
using TeamAPI.Services;

namespace TeamAPI.Controllers;

/// <summary>
/// Controller for managing teams.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class TeamController(
    ITeamService teamService, 
    IConnectionMultiplexer redis, 
    ILogger<TeamController> logger) : ControllerBase
{
    private readonly IDatabase _cache = redis.GetDatabase();
    private const int CacheExpirationInSeconds = 60;
    
    /// <summary>
    /// Retrieves all teams with Redis caching.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
    {
        const string cacheKey = "teams:all";
        try
        {
            logger.LogInformation("Retrieving all teams.");

            // Check if data exists in the Redis cache
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                var cachedTeams = JsonSerializer.Deserialize<IEnumerable<TeamDto>>(cachedData);
                logger.LogInformation("Teams retrieved from cache.");
                return Ok(cachedTeams);
            }

            // Retrieve data from the database if not in cache
            var teams = await teamService.GetAllTeamsAsync();

            // Cache the response
            var serializedTeams = JsonSerializer.Serialize(teams);
            await _cache.StringSetAsync(cacheKey, serializedTeams, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            logger.LogInformation("Teams retrieved from database and cached.");
            return Ok(teams);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all teams.");
            return StatusCode(500, "Internal server error.");
        }
    }

    /// <summary>
    /// Retrieves a team by ID with Redis caching.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDetailsDto>> GetTeam(int id)
    {
        var cacheKey = $"teams:{id}";
        try
        {
            logger.LogInformation("Retrieving team with ID {Id}.", id);

            // Check if data exists in the Redis cache
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                var cachedTeam = JsonSerializer.Deserialize<TeamDetailsDto>(cachedData);
                logger.LogInformation("Team with ID {Id} retrieved from cache.", id);
                return Ok(cachedTeam);
            }

            // Retrieve data from the database if not in cache
            var team = await teamService.GetTeamByIdAsync(id);

            // Cache the response
            var serializedTeam = JsonSerializer.Serialize(team);
            await _cache.StringSetAsync(cacheKey, serializedTeam, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            logger.LogInformation("Team with ID {Id} retrieved from database and cached.", id);
            return Ok(team);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving team with ID {Id}.", id);
            return StatusCode(500, "Internal server error.");
        }
    }

    /// <summary>
    /// Creates a new team and clears the cached list of all teams.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TeamDto>> CreateTeam(TeamDto teamDto)
    {
        try
        {
            logger.LogInformation("Creating a new team.");
            var team = await teamService.CreateTeamAsync(teamDto);

            // Invalidate the cache for the list of teams
            await _cache.KeyDeleteAsync("teams:all");

            logger.LogInformation("New team created and cache invalidated.");
            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating a new team.");
            return StatusCode(500, "Internal server error.");
        }
    }

    /// <summary>
    /// Updates a team and clears its cache.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TeamDto>> UpdateTeam(int id, TeamDto teamDto)
    {
        try
        {
            logger.LogInformation("Updating team with ID {Id}.", id);
            var team = await teamService.UpdateTeamAsync(id, teamDto);

            // Invalidate the cache for this specific team and the list of teams
            var cacheKey = $"teams:{id}";
            await _cache.KeyDeleteAsync(cacheKey);
            await _cache.KeyDeleteAsync("teams:all");

            logger.LogInformation("Team with ID {Id} updated and cache invalidated.", id);
            return Ok(team);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating team with ID {Id}.", id);
            return StatusCode(500, "Internal server error.");
        }
    }

    /// <summary>
    /// Deletes a team and clears its cache.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeam(int id)
    {
        try
        {
            logger.LogInformation("Deleting team with ID {Id}.", id);
            await teamService.DeleteTeamAsync(id);

            // Invalidate the cache for this specific team and the list of teams
            var cacheKey = $"teams:{id}";
            await _cache.KeyDeleteAsync(cacheKey);
            await _cache.KeyDeleteAsync("teams:all");

            logger.LogInformation("Team with ID {Id} deleted and cache invalidated.", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting team with ID {Id}.", id);
            return StatusCode(500, "Internal server error.");
        }
    }

    /// <summary>
    /// Adds a user to a team and clears the team's cache.
    /// </summary>
    [HttpPost("{id}/{userId}")]
    public async Task<ActionResult> AddUserToTeam(int id, int userId)
    {
        try
        {
            logger.LogInformation("Adding user with ID {UserId} to team with ID {TeamId}.", userId, id);
            await teamService.AddUserToTeamAsync(id, userId);

            // Invalidate the cache for this specific team
            var cacheKey = $"teams:{id}";
            await _cache.KeyDeleteAsync(cacheKey);

            logger.LogInformation("User with ID {UserId} added to team with ID {TeamId} and cache invalidated.", userId, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding user with ID {UserId} to team with ID {TeamId}.", userId, id);
            return StatusCode(500, "Internal server error.");
        }
    }

    /// <summary>
    /// Removes a user from a team and clears the team's cache.
    /// </summary>
    [HttpDelete("{id}/{userId}")]
    public async Task<ActionResult> RemoveUserFromTeam(int id, int userId)
    {
        try
        {
            logger.LogInformation("Removing user with ID {UserId} from team with ID {TeamId}.", userId, id);
            await teamService.RemoveUserFromTeamAsync(id, userId);

            // Invalidate the cache for this specific team
            var cacheKey = $"teams:{id}";
            await _cache.KeyDeleteAsync(cacheKey);

            logger.LogInformation("User with ID {UserId} removed from team with ID {TeamId} and cache invalidated.", userId, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing user with ID {UserId} from team with ID {TeamId}.", userId, id);
            return StatusCode(500, "Internal server error.");
        }
    }
}