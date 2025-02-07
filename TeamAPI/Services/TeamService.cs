using AutoMapper;
using FluentValidation;
using TeamAPI.Models;
using TeamAPI.Models.DTOs;
using TeamAPI.Repositories;

namespace TeamAPI.Services;

/// <summary>
/// Service class for managing teams.
/// </summary>
public class TeamService(ITeamRepository teamRepository, IMapper mapper, ILogger<TeamService> logger) : ITeamService
{
    /// <summary>
    /// Creates a new team asynchronously.
    /// </summary>
    /// <param name="teamDto">The team DTO to create.</param>
    /// <returns>The created team DTO.</returns>
    public async Task<TeamDto> CreateTeamAsync(TeamDto teamDto)
    {
        try
        {
            logger.LogInformation("Creating a new team.");

            // Validation
            var validator = new TeamDtoValidator();
            var validationResult = await validator.ValidateAsync(teamDto);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors;
                throw new ValidationException(errorMessages);
            }

            // Mapping
            var team = mapper.Map<Team>(teamDto);

            // Database persistence
            await teamRepository.AddAsync(team);

            logger.LogInformation("Team created successfully.");
            return mapper.Map<TeamDto>(team);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Validation error while creating team.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating team.");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a team by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the team to retrieve.</param>
    /// <returns>The team details DTO.</returns>
    public async Task<TeamDetailsDto> GetTeamByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Retrieving team with ID {Id}.", id);
            var team = await teamRepository.GetByIdAsync(id);
            if (team == null)
            {
                logger.LogWarning("Team with ID {Id} not found.", id);
                throw new KeyNotFoundException("Team not found.");
            }
            logger.LogInformation("Team with ID {Id} retrieved successfully.", id);
            return mapper.Map<TeamDetailsDto>(team);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving team with ID {Id}.", id);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all teams asynchronously.
    /// </summary>
    /// <returns>A list of all team DTOs.</returns>
    public async Task<List<TeamDto>> GetAllTeamsAsync()
    {
        try
        {
            logger.LogInformation("Retrieving all teams.");
            var teams = await teamRepository.GetAllAsync();
            logger.LogInformation("All teams retrieved successfully.");
            return mapper.Map<List<TeamDto>>(teams);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all teams.");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing team asynchronously.
    /// </summary>
    /// <param name="id">The ID of the team to update.</param>
    /// <param name="teamDto">The team DTO to update.</param>
    /// <returns>The updated team DTO.</returns>
    public async Task<TeamDto> UpdateTeamAsync(int id, TeamDto teamDto)
    {
        try
        {
            logger.LogInformation("Updating team with ID {Id}.", id);

            // Validation
            var validator = new TeamDtoValidator();
            var validationResult = await validator.ValidateAsync(teamDto);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors;
                throw new ValidationException(errorMessages);
            }

            // Get the team from the database
            var team = await teamRepository.GetByIdAsync(id);
            if (team == null)
            {
                logger.LogWarning("Team with ID {Id} not found.", id);
                throw new KeyNotFoundException("Team not found.");
            }

            // Update team properties
            mapper.Map(teamDto, team);

            // Database persistence
            await teamRepository.UpdateAsync(team);

            logger.LogInformation("Team with ID {Id} updated successfully.", id);
            return mapper.Map<TeamDto>(team);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Validation error while updating team with ID {Id}.", id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating team with ID {Id}.", id);
            throw;
        }
    }

    /// <summary>
    /// Deletes a team by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the team to delete.</param>
    public async Task DeleteTeamAsync(int id)
    {
        try
        {
            logger.LogInformation("Deleting team with ID {Id}.", id);
            await teamRepository.DeleteAsync(id);
            logger.LogInformation("Team with ID {Id} deleted successfully.", id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting team with ID {Id}.", id);
            throw;
        }
    }

    /// <summary>
    /// Adds a user to a team asynchronously.
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user.</param>
    public async Task AddUserToTeamAsync(int teamId, int userId)
    {
        try
        {
            logger.LogInformation("Adding user with ID {UserId} to team with ID {TeamId}.", userId, teamId);

            // Validate if the team exists
            var team = await teamRepository.GetByIdAsync(teamId);
            if (team == null)
            {
                logger.LogWarning("Team with ID {TeamId} not found.", teamId);
                throw new KeyNotFoundException("Team not found.");
            }

            // Add the user to the team
            await teamRepository.AddUserToTeamAsync(teamId, userId);
            logger.LogInformation("User with ID {UserId} added to team with ID {TeamId} successfully.", userId, teamId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding user with ID {UserId} to team with ID {TeamId}.", userId, teamId);
            throw;
        }
    }

    /// <summary>
    /// Removes a user from a team asynchronously.
    /// </summary>
    /// <param name="teamId">The ID of the team.</param>
    /// <param name="userId">The ID of the user.</param>
    public async Task RemoveUserFromTeamAsync(int teamId, int userId)
    {
        try
        {
            logger.LogInformation("Removing user with ID {UserId} from team with ID {TeamId}.", userId, teamId);

            // Validate if the team exists
            var team = await teamRepository.GetByIdAsync(teamId);
            if (team == null)
            {
                logger.LogWarning("Team with ID {TeamId} not found.", teamId);
                throw new KeyNotFoundException("Team not found.");
            }

            // Remove the user from the team
            await teamRepository.RemoveUserFromTeamAsync(teamId, userId);
            logger.LogInformation("User with ID {UserId} removed from team with ID {TeamId} successfully.", userId, teamId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing user with ID {UserId} from team with ID {TeamId}.", userId, teamId);
            throw;
        }
    }
}