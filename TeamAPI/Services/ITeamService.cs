using TeamAPI.Models.DTOs;

namespace TeamAPI.Services;

public interface ITeamService
{
    Task<TeamDto> CreateTeamAsync(TeamDto teamDto);
    Task<TeamDto> GetTeamByIdAsync(int id);
    Task<List<TeamDto>> GetAllTeamsAsync();
    Task<TeamDto> UpdateTeamAsync(int id, TeamDto teamDto);
    Task DeleteTeamAsync(int id);
    Task AddUserToTeamAsync(int teamId, int userId);
    Task RemoveUserFromTeamAsync(int teamId, int userId);
}