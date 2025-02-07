using ClockAPI.Models.DTOs;
using ClockAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace ClockAPI.Controllers;

/// <summary>
///     Controller for managing time entries.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class TimeEntriesController(
    ITimeEntryService timeEntryService,
    IConnectionMultiplexer redis) : Controller
{
    private readonly IDatabase _cache = redis.GetDatabase();
    private const int CacheExpirationInSeconds = 60; // Cache expiration time (1 minute)

    /// <summary>
    ///     Gets all time entries.
    /// </summary>
    /// <returns>A list of time entries.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntries()
    {
        const string cacheKey = "timeentries:all";

        try
        {
            // Check if data exists in the Redis cache
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                var cachedTimeEntries = JsonSerializer.Deserialize<IEnumerable<TimeEntryDto>>(cachedData);
                return Ok(cachedTimeEntries);
            }

            // Retrieve data from the database if not in cache
            var timeEntries = await timeEntryService.GetAllTimeEntriesAsync();

            // Cache the response
            var serializedTimeEntries = JsonSerializer.Serialize(timeEntries);
            await _cache.StringSetAsync(cacheKey, serializedTimeEntries, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            return Ok(timeEntries);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Gets a specific time entry by ID.
    /// </summary>
    /// <param name="id">The ID of the time entry.</param>
    /// <returns>The time entry with the specified ID.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntryDto>> GetTimeEntry(int id)
    {
        var cacheKey = $"timeentries:{id}";

        try
        {
            // Check if data exists in the Redis cache
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                var cachedTimeEntry = JsonSerializer.Deserialize<TimeEntryDto>(cachedData);
                return Ok(cachedTimeEntry);
            }

            // Retrieve data from the database if not in cache
            var timeEntry = await timeEntryService.GetTimeEntryByIdAsync(id);

            // Cache the response
            var serializedTimeEntry = JsonSerializer.Serialize(timeEntry);
            await _cache.StringSetAsync(cacheKey, serializedTimeEntry, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            return Ok(timeEntry);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Gets time entries for a specific user by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of time entries for the specified user.</returns>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntriesByUserId(int userId)
    {
        var cacheKey = $"timeentries:user:{userId}";

        try
        {
            // Check if data exists in the Redis cache
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                var cachedUserTimeEntries = JsonSerializer.Deserialize<IEnumerable<TimeEntryDto>>(cachedData);
                return Ok(cachedUserTimeEntries);
            }

            // Retrieve data from the database if not in cache
            var timeEntries = await timeEntryService.GetTimeEntriesByUserIdAsync(userId);

            // Cache the response
            var serializedTimeEntries = JsonSerializer.Serialize(timeEntries);
            await _cache.StringSetAsync(cacheKey, serializedTimeEntries, TimeSpan.FromSeconds(CacheExpirationInSeconds));

            return Ok(timeEntries);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Creates a new time entry.
    /// </summary>
    /// <param name="timeEntry">The time entry to create.</param>
    /// <returns>The created time entry.</returns>
    [HttpPost]
    public async Task<ActionResult<TimeEntryDto>> PostTimeEntry(TimeEntryDto timeEntry)
    {
        if (!ModelState.IsValid) return BadRequest();

        try
        {
            await timeEntryService.AddTimeEntryAsync(timeEntry);

            // Invalidate relevant caches
            await _cache.KeyDeleteAsync("timeentries:all");
            await _cache.KeyDeleteAsync($"timeentries:user:{timeEntry.UserId}");

            return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, timeEntry);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Updates an existing time entry.
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

            // Invalidate relevant caches
            var cacheKey = $"timeentries:{id}";
            await _cache.KeyDeleteAsync(cacheKey);
            await _cache.KeyDeleteAsync("timeentries:all");
            await _cache.KeyDeleteAsync($"timeentries:user:{timeEntry.UserId}");

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Deletes a specific time entry by ID.
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

            // Invalidate relevant caches
            var cacheKey = $"timeentries:{id}";
            await _cache.KeyDeleteAsync(cacheKey);
            await _cache.KeyDeleteAsync("timeentries:all");
            await _cache.KeyDeleteAsync($"timeentries:user:{timeEntry.UserId}");

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
