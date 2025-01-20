using AutoMapper;
using ClockAPI.Models;
using ClockAPI.Models.DTOs;
using ClockAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClockAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class TimeEntriesController(
    ITimeEntryService timeEntryService,
    IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntries()
    {
        var timeEntries = await timeEntryService.GetAllTimeEntriesAsync();
        return Ok(mapper.Map<IEnumerable<TimeEntryDto>>(timeEntries));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntryDto>> GetTimeEntry(int id)
    {
        return await timeEntryService.GetTimeEntryByIdAsync(id) is { } timeEntry ? mapper.Map<TimeEntryDto>(timeEntry) : NotFound();
    }
    
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TimeEntryDto>>> GetTimeEntriesByUserId(int userId)
    {
        var timeEntries = await timeEntryService.GetTimeEntriesByUserIdAsync(userId);
        return Ok(mapper.Map<IEnumerable<TimeEntryDto>>(timeEntries));
    }
    
    [HttpPost]
    public async Task<ActionResult<TimeEntryDto>> PostTimeEntry(TimeEntryDto timeEntry)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        await timeEntryService.AddTimeEntryAsync(mapper.Map<TimeEntry>(timeEntry));
        return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, timeEntry);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTimeEntry(int id, TimeEntryDto timeEntry)
    {
        if (id != timeEntry.Id)
        {
            return BadRequest();
        }

        await timeEntryService.UpdateTimeEntryAsync(mapper.Map<TimeEntry>(timeEntry));

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTimeEntry(int id)
    {
        await timeEntryService.DeleteTimeEntryAsync(id);
        return NoContent();
    }
}