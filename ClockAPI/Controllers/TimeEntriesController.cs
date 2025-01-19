using ClockAPI.Models;
using ClockAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClockAPI.Controllers;

public class TimeEntriesController(TimeEntryService timeEntryService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntries()
    {
        return await timeEntryService.GetAllTimeEntriesAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntry>> GetTimeEntry(int id)
    {
        return await timeEntryService.GetTimeEntryByIdAsync(id) is { } timeEntry ? timeEntry : NotFound();
    }
    
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntriesByUserId(int userId)
    {
        return await timeEntryService.GetTimeEntriesByUserIdAsync(userId);
    }
    
    [HttpPost]
    public async Task<ActionResult<TimeEntry>> PostTimeEntry(TimeEntry timeEntry)
    {
        await timeEntryService.AddTimeEntryAsync(timeEntry);
        return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, timeEntry);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTimeEntry(int id, TimeEntry timeEntry)
    {
        if (id != timeEntry.Id)
        {
            return BadRequest();
        }

        await timeEntryService.UpdateTimeEntryAsync(timeEntry);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTimeEntry(int id)
    {
        await timeEntryService.DeleteTimeEntryAsync(id);
        return NoContent();
    }
}