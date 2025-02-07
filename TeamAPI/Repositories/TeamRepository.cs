using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamAPI.Repositories;

/// <summary>
/// Repository for managing team data.
/// </summary>
public class TeamRepository(TeamDbContext dbContext, ILogger<TeamRepository> logger) : ITeamRepository
{
    /// <inheritdoc />
    public async Task<Team> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Retrieving team with ID {Id}.", id);
            var team = await dbContext.TeamEntries
                .Include(t => t.TeamMembers)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                logger.LogWarning("Team with ID {Id} not found.", id);
                throw new KeyNotFoundException("Team not found.");
            }
            logger.LogInformation("Team with ID {Id} retrieved successfully.", id);
            return team;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving team with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<Team>> GetAllAsync()
    {
        try
        {
            logger.LogInformation("Retrieving all teams.");
            var teams = await dbContext.TeamEntries
                .Include(t => t.TeamMembers)
                .ToListAsync();
            logger.LogInformation("All teams retrieved successfully.");
            return teams;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all teams.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task AddAsync(Team team)
    {
        try
        {
            logger.LogInformation("Adding a new team.");
            await dbContext.TeamEntries.AddAsync(team);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Team added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding team.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Team team)
    {
        try
        {
            logger.LogInformation("Updating team with ID {Id}.", team.Id);
            dbContext.TeamEntries.Update(team);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Team with ID {Id} updated successfully.", team.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating team with ID {Id}.", team.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        try
        {
            logger.LogInformation("Deleting team with ID {Id}.", id);
            var team = await GetByIdAsync(id);
            dbContext.TeamEntries.Remove(team);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Team with ID {Id} deleted successfully.", id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting team with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task AddUserToTeamAsync(int teamId, int userId)
    {
        try
        {
            logger.LogInformation("Adding user with ID {UserId} to team with ID {TeamId}.", userId, teamId);
            var teamMember = new TeamMember { TeamId = teamId, UserId = userId };
            await dbContext.TeamMemberEntries.AddAsync(teamMember);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("User with ID {UserId} added to team with ID {TeamId} successfully.", userId, teamId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding user with ID {UserId} to team with ID {TeamId}.", userId, teamId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task RemoveUserFromTeamAsync(int teamId, int userId)
    {
        try
        {
            logger.LogInformation("Removing user with ID {UserId} from team with ID {TeamId}.", userId, teamId);
            var teamMember = await dbContext.TeamMemberEntries
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
            if (teamMember == null)
            {
                logger.LogWarning("User with ID {UserId} not found in team with ID {TeamId}.", userId, teamId);
                throw new KeyNotFoundException("User not found in team.");
            }
            dbContext.TeamMemberEntries.Remove(teamMember);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("User with ID {UserId} removed from team with ID {TeamId} successfully.", userId, teamId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing user with ID {UserId} from team with ID {TeamId}.", userId, teamId);
            throw;
        }
    }
}