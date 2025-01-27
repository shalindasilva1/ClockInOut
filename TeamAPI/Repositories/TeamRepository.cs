using Microsoft.EntityFrameworkCore;
using TeamAPI.Models;

namespace TeamAPI.Repositories;

public class TeamRepository(TeamDbContext dbContext) : ITeamRepository
{
    public async Task<Team> GetByIdAsync(int id)
    {
        return await dbContext.TeamEntries
            .Include(t => t.TeamMembers)
            .FirstOrDefaultAsync(t => t.Id == id) ?? new Team();
    }

    public async Task<List<Team>> GetAllAsync()
    {
        return await dbContext.TeamEntries
            .Include(t => t.TeamMembers)
            .ToListAsync();
    }

    public async Task AddAsync(Team team)
    {
        dbContext.TeamEntries.Add(team);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Team team)
    {
        dbContext.TeamEntries.Update(team);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var team = await GetByIdAsync(id);
        if (team.Id > 0)
        {
            dbContext.TeamEntries.Remove(team);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task AddUserToTeamAsync(int teamId, int userId)
    {
        var teamMember = new TeamMember { TeamId = teamId, UserId = userId };
        dbContext.TeamMemberEntries.Add(teamMember);
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveUserFromTeamAsync(int teamId, int userId)
    {
        var teamMember = await dbContext.TeamMemberEntries
            .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);

        if (teamMember != null)
        {
            dbContext.TeamMemberEntries.Remove(teamMember);
            await dbContext.SaveChangesAsync();
        }
    }
}