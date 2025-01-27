using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
using TeamAPI.Models.DTOs;
using TeamAPI.Services;

namespace TeamAPI.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TeamController(
    ITeamService teamService,
    IConnectionMultiplexer redis) : Controller
{
    private readonly IDatabase _cache = redis.GetDatabase();
    private const int CacheExpirationInSeconds = 60; // Cache expiration time (1 minute)

    /// <summary>
    /// Retrieves all teams with Redis caching.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
    {
        const string cacheKey = "teams:all";
        
        // Check if data exists in the Redis cache
        var cachedData = await _cache.StringGetAsync(cacheKey);
        if (!cachedData.IsNullOrEmpty)
        {
            var cachedTeams = JsonSerializer.Deserialize<IEnumerable<TeamDto>>(cachedData);
            return Ok(cachedTeams);
        }

        // Retrieve data from the database if not in cache
        var teams = await teamService.GetAllTeamsAsync();

        // Cache the response
        var serializedTeams = JsonSerializer.Serialize(teams);
        await _cache.StringSetAsync(cacheKey, serializedTeams, TimeSpan.FromSeconds(CacheExpirationInSeconds));

        return Ok(teams);
    }

    /// <summary>
    /// Retrieves a team by ID with Redis caching.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDetailsDto>> GetTeam(int id)
    {
        var cacheKey = $"teams:{id}";

        // Check if data exists in the Redis cache
        var cachedData = await _cache.StringGetAsync(cacheKey);
        if (!cachedData.IsNullOrEmpty)
        {
            var cachedTeam = JsonSerializer.Deserialize<TeamDetailsDto>(cachedData);
            return Ok(cachedTeam);
        }

        try
        {
            // Retrieve data from the database if not in cache
            var team = await teamService.GetTeamByIdAsync(id);

            // Cache the response
            var serializedTeam = JsonSerializer.Serialize(team);
            await _cache.StringSetAsync(cacheKey, serializedTeam, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            return Ok(team);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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
            var team = await teamService.CreateTeamAsync(teamDto);

            // Invalidate the cache for the list of teams
            await _cache.KeyDeleteAsync("teams:all");

            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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
            var team = await teamService.UpdateTeamAsync(id, teamDto);

            // Invalidate the cache for this specific team and the list of teams
            var cacheKey = $"teams:{id}";
            await _cache.KeyDeleteAsync(cacheKey);
            await _cache.KeyDeleteAsync("teams:all");

            return Ok(team);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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
            await teamService.DeleteTeamAsync(id);

            // Invalidate the cache for this specific team and the list of teams
            var cacheKey = $"teams:{id}";
            await _cache.KeyDeleteAsync(cacheKey);
            await _cache.KeyDeleteAsync("teams:all");

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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
            await teamService.AddUserToTeamAsync(id, userId);

            // Invalidate the cache for this specific team
            var cacheKey = $"teams:{id}";
            await _cache.KeyDeleteAsync(cacheKey);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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
            await teamService.RemoveUserFromTeamAsync(id, userId);

            // Invalidate the cache for this specific team
            var cacheKey = $"teams:{id}";
            await _cache.KeyDeleteAsync(cacheKey);

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
