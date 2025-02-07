using ClockAPI.Models.DTOs;
using ClockAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;
    
namespace ClockAPI.Controllers;

/// <summary>
/// Controller for managing time entries.
/// </summary>
/// <param name="timeEntryService">Service for handling time entry operations.</param>
/// <param name="logger">Logger for logging information and errors.</param>
/// <param name="redis">Redis connection multiplexer for caching.</param>
[ApiController]
[Authorize]
[Route("[controller]")]
public class TimeEntriesController(
    ITimeEntryService timeEntryService,
    ILogger<TimeEntriesController> logger,
    IConnectionMultiplexer redis) : ControllerBase
{
    private readonly IDatabase _cache = redis.GetDatabase();
    private const int CacheExpirationInSeconds = 60;

    /// <summary>
    /// Gets all time entries.
    /// </summary>
    /// <returns>A list of time entries.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntries()
    {
        const string cacheKey = "timeentries:all";

        try
        {
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                var cachedTimeEntries = JsonSerializer.Deserialize<IEnumerable<TimeEntryDto>>(cachedData);
                return Ok(cachedTimeEntries);
            }

            var timeEntries = await timeEntryService.GetAllTimeEntriesAsync();
            var serializedTimeEntries = JsonSerializer.Serialize(timeEntries);
            await _cache.StringSetAsync(cacheKey, serializedTimeEntries, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            return Ok(timeEntries);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting time entries.");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets a time entry by ID.
    /// </summary>
    /// <param name="id">The ID of the time entry.</param>
    /// <returns>The time entry with the specified ID.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntryDto>> GetTimeEntry(int id)
    {
        var cacheKey = $"timeentries:{id}";

        try
        {
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                var cachedTimeEntry = JsonSerializer.Deserialize<TimeEntryDto>(cachedData);
                return Ok(cachedTimeEntry);
            }

            var timeEntry = await timeEntryService.GetTimeEntryByIdAsync(id);
            var serializedTimeEntry = JsonSerializer.Serialize(timeEntry);
            await _cache.StringSetAsync(cacheKey, serializedTimeEntry, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            return Ok(timeEntry);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error occurred while getting time entry with ID {id}.");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets time entries by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of time entries for the specified user.</returns>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntriesByUserId(int userId)
    {
        var cacheKey = $"timeentries:user:{userId}";

        try
        {
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                var cachedUserTimeEntries = JsonSerializer.Deserialize<IEnumerable<TimeEntryDto>>(cachedData);
                return Ok(cachedUserTimeEntries);
            }

            var timeEntries = await timeEntryService.GetTimeEntriesByUserIdAsync(userId);
            var serializedTimeEntries = JsonSerializer.Serialize(timeEntries);
            await _cache.StringSetAsync(cacheKey, serializedTimeEntries, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            return Ok(timeEntries);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error occurred while getting time entries for user ID {userId}.");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Adds a new time entry.
    /// </summary>
    /// <param name="timeEntry">The time entry to add.</param>
    /// <returns>The created time entry.</returns>
    [HttpPost]
    public async Task<ActionResult<TimeEntryDto>> PostTimeEntry(TimeEntryDto timeEntry)
    {
        if (!ModelState.IsValid) return BadRequest();

        try
        {
            await timeEntryService.AddTimeEntryAsync(timeEntry);

            await _cache.KeyDeleteAsync("timeentries:all");
            await _cache.KeyDeleteAsync($"timeentries:user:{timeEntry.UserId}");

            return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, timeEntry);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while adding a new time entry.");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing time entry.
    /// </summary>
    /// <param name="id">The ID of the time entry to update.</param>
    /// <param name="timeEntry">The updated time entry.</param>
    /// <returns>No content.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTimeEntry(int id, TimeEntryDto timeEntry)
    {
        if (id != timeEntry.Id) return BadRequest();

        try
        {
            await timeEntryService.UpdateTimeEntryAsync(timeEntry);

            var cacheKey = $"timeentries:{id}";
            await _cache.KeyDeleteAsync(cacheKey);
            await _cache.KeyDeleteAsync("timeentries:all");
            await _cache.KeyDeleteAsync($"timeentries:user:{timeEntry.UserId}");

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error occurred while updating time entry with ID {id}.");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deletes a time entry.
    /// </summary>
    /// <param name="id">The ID of the time entry to delete.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTimeEntry(int id)
    {
        try
        {
            var timeEntry = await timeEntryService.GetTimeEntryByIdAsync(id);
            await timeEntryService.DeleteTimeEntryAsync(id);

            var cacheKey = $"timeentries:{id}";
            await _cache.KeyDeleteAsync(cacheKey);
            await _cache.KeyDeleteAsync("timeentries:all");
            await _cache.KeyDeleteAsync($"timeentries:user:{timeEntry.UserId}");

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error occurred while deleting time entry with ID {id}.");
            return StatusCode(500, "Internal server error");
        }
    }
}