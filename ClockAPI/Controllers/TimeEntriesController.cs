using AutoMapper;
using ClockAPI.Models;
using ClockAPI.Models.DTOs;
using ClockAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClockAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class TimeEntriesController(ITimeEntryService timeEntryService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntries()
    {
        var timeEntries = await timeEntryService.GetAllTimeEntriesAsync();
        return Ok(timeEntries);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntryDto>> GetTimeEntry(int id)
    {
        return await timeEntryService.GetTimeEntryByIdAsync(id) is { } timeEntry ? timeEntry : NotFound();
    }
    
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntriesByUserId(int userId)
    {
        var timeEntries = await timeEntryService.GetTimeEntriesByUserIdAsync(userId);
        return Ok(timeEntries);
    }
    
    [HttpPost]
    public async Task<ActionResult<TimeEntryDto>> PostTimeEntry(TimeEntryDto timeEntry)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        await timeEntryService.AddTimeEntryAsync(timeEntry);
        return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, timeEntry);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTimeEntry(int id, TimeEntryDto timeEntry)
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