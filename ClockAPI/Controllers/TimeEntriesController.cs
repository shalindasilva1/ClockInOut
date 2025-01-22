using AutoMapper;
using ClockAPI.Models;
using ClockAPI.Models.DTOs;
using ClockAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClockAPI.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class TimeEntriesController(ITimeEntryService timeEntryService) : Controller
{
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

    [HttpGet("{id}")]
    public async Task<ActionResult<TimeEntryDto>> GetTimeEntry(int id)
    {
        try
        {
            var timeEntry = await timeEntryService.GetTimeEntryByIdAsync(id);
            return timeEntry;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
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
    
    [HttpPost]
    public async Task<ActionResult<TimeEntryDto>> PostTimeEntry(TimeEntryDto timeEntry)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

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
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTimeEntry(int id, TimeEntryDto timeEntry)
    {
        if (id != timeEntry.Id)
        {
            return BadRequest();
        }
        
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