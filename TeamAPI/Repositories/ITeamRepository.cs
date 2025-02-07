using TeamAPI.Models;
    
namespace TeamAPI.Repositories;

/// <summary>
/// Interface for managing team data.
/// </summary>
public interface ITeamRepository
{
    /// <summary>
    /// Retrieves a team by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the team to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the team.</returns>
    Task<Team> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all teams asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all teams.</returns>
    Task<List<Team>> GetAllAsync();

    /// <summary>
    /// Adds a new team asynchronously.
    /// </summary>
    /// <param name="team">The team to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(Team team);

    /// <summary>
    /// Updates an existing team asynchronously.
    /// </summary>
    /// <param name="team">The team to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(Team team);

    /// <summary>
    /// Deletes a team by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the team to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// Adds a user to a team asynchronously.
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddUserToTeamAsync(int teamId, int userId);

    /// <summary>
    /// Removes a user from a team asynchronously.
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveUserFromTeamAsync(int teamId, int userId);
}