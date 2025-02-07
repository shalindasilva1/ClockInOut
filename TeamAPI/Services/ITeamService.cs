using TeamAPI.Models.DTOs;
    
namespace TeamAPI.Services;

/// <summary>
/// Interface for managing team operations.
/// </summary>
public interface ITeamService
{
    /// <summary>
    /// Creates a new team asynchronously.
    /// </summary>
    /// <param name="teamDto">The team DTO to create.</param>
    /// <returns>The created team DTO.</returns>
    Task<TeamDto> CreateTeamAsync(TeamDto teamDto);

    /// <summary>
    /// Retrieves a team by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the team to retrieve.</param>
    /// <returns>The team details DTO.</returns>
    Task<TeamDetailsDto> GetTeamByIdAsync(int id);

    /// <summary>
    /// Retrieves all teams asynchronously.
    /// </summary>
    /// <returns>A list of all team DTOs.</returns>
    Task<List<TeamDto>> GetAllTeamsAsync();

    /// <summary>
    /// Updates an existing team asynchronously.
    /// </summary>
    /// <param name="id">The ID of the team to update.</param>
    /// <param name="teamDto">The team DTO to update.</param>
    /// <returns>The updated team DTO.</returns>
    Task<TeamDto> UpdateTeamAsync(int id, TeamDto teamDto);

    /// <summary>
    /// Deletes a team by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the team to delete.</param>
    Task DeleteTeamAsync(int id);

    /// <summary>
    /// Adds a user to a team asynchronously.
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user.</param>
    Task AddUserToTeamAsync(int teamId, int userId);

    /// <summary>
    /// Removes a user from a team asynchronously.
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user.</param>
    Task RemoveUserFromTeamAsync(int teamId, int userId);
}