using TeamAPI.Models;

namespace TeamAPI.Repositories;

public interface ITeamRepository
{
    Task<Team> GetByIdAsync(int id);
    Task<List<Team>> GetAllAsync();
    Task AddAsync(Team team);
    Task UpdateAsync(Team team);
    Task DeleteAsync(int id);
    Task AddUserToTeamAsync(int teamId, int userId);
    Task RemoveUserFromTeamAsync(int teamId, int userId);
}