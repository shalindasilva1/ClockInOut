using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamAPI.Models.DTOs;
using TeamAPI.Services;

namespace TeamAPI.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TeamController(ITeamService teamService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
    {
        var teams = await teamService.GetAllTeamsAsync();
        return Ok(teams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDto>> GetTeam(int id)
    {
        try
        {
            var team = await teamService.GetTeamByIdAsync(id);
            return Ok(team);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<TeamDto>> CreateTeam(TeamDto teamDto)
    {
        try
        {
            var team = await teamService.CreateTeamAsync(teamDto);
            return CreatedAtAction(nameof(GetTeam), team);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<TeamDto>> UpdateTeam(int id, TeamDto teamDto)
    {
        try
        {
            var team = await teamService.UpdateTeamAsync(id, teamDto);
            return Ok(team);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeam(int id)
    {
        try
        {
            await teamService.DeleteTeamAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("{id}/{userId}")]
    public async Task<ActionResult> AddUserToTeam(int id, int userId)
    {
        try
        {
            await teamService.AddUserToTeamAsync(id, userId);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}/{userId}")]
    public async Task<ActionResult> RemoveUserFromTeam(int id, int userId)
    {
        try
        {
            await teamService.RemoveUserFromTeamAsync(id, userId);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}