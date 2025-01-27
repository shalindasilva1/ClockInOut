using ClockAPI.Models.DTOs;
using ClockAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClockAPI.Controllers;

/// <summary>
///     Controller for managing time entries.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class TimeEntriesController(ITimeEntryService timeEntryService) : Controller
{
    /// <summary>
    ///     Gets all time entries.
    /// </summary>
    /// <returns>A list of time entries.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntries()
    {
        try
        {
            var timeEntries = await timeEntryService.GetAllTimeEntriesAsync();
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
        try
        {
            var timeEntry = await timeEntryService.GetTimeEntryByIdAsync(id);
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
        try
        {
            var timeEntries = await timeEntryService.GetTimeEntriesByUserIdAsync(userId);
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
            await timeEntryService.DeleteTimeEntryAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}