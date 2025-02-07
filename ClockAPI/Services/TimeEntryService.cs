using AutoMapper;
using ClockAPI.Models;
using ClockAPI.Models.DTOs;
using ClockAPI.Repositories;
        
namespace ClockAPI.Services;

/// <summary>
/// Service class for managing time entries.
/// </summary>
public class TimeEntryService(
    ITimeEntryRepository timeEntryRepository, 
    IMapper mapper, 
    ILogger<TimeEntryService> logger) : ITimeEntryService
{
    /// <summary>
    /// Adds a new time entry asynchronously.
    /// </summary>
    /// <param name="timeEntry">The time entry DTO to add.</param>
    public async Task AddTimeEntryAsync(TimeEntryDto timeEntry)
    {
        try
        {
            logger.LogInformation("Adding a new time entry.");
            await timeEntryRepository.AddAsync(mapper.Map<TimeEntry>(timeEntry));
            logger.LogInformation("Time entry added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding time entry.");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing time entry asynchronously.
    /// </summary>
    /// <param name="timeEntry">The time entry DTO to update.</param>
    public async Task UpdateTimeEntryAsync(TimeEntryDto timeEntry)
    {
        try
        {
            logger.LogInformation("Updating time entry with ID {Id}.", timeEntry.Id);
            await timeEntryRepository.UpdateAsync(mapper.Map<TimeEntry>(timeEntry));
            logger.LogInformation("Time entry updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating time entry with ID {Id}.", timeEntry.Id);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a time entry by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the time entry to retrieve.</param>
    /// <returns>The time entry DTO.</returns>
    public async Task<TimeEntryDto> GetTimeEntryByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Retrieving time entry with ID {Id}.", id);
            var result = await timeEntryRepository.GetByIdAsync(id);
            logger.LogInformation("Time entry retrieved successfully.");
            return mapper.Map<TimeEntryDto>(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving time entry with ID {Id}.", id);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all time entries asynchronously.
    /// </summary>
    /// <returns>A list of all time entry DTOs.</returns>
    public async Task<List<TimeEntryDto>> GetAllTimeEntriesAsync()
    {
        try
        {
            logger.LogInformation("Retrieving all time entries.");
            var result = await timeEntryRepository.GetAllAsync();
            logger.LogInformation("All time entries retrieved successfully.");
            return mapper.Map<List<TimeEntryDto>>(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all time entries.");
            throw;
        }
    }

    /// <summary>
    /// Retrieves time entries by user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user ID to filter time entries.</param>
    /// <returns>A list of time entry DTOs for the specified user.</returns>
    public async Task<List<TimeEntryDto>> GetTimeEntriesByUserIdAsync(int userId)
    {
        try
        {
            logger.LogInformation("Retrieving time entries for user ID {UserId}.", userId);
            var result = await timeEntryRepository.GetByUserIdAsync(userId);
            logger.LogInformation("Time entries for user ID {UserId} retrieved successfully.", userId);
            return mapper.Map<List<TimeEntryDto>>(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving time entries for user ID {UserId}.", userId);
            throw;
        }
    }

    /// <summary>
    /// Deletes a time entry by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the time entry to delete.</param>
    public async Task DeleteTimeEntryAsync(int id)
    {
        try
        {
            logger.LogInformation("Deleting time entry with ID {Id}.", id);
            await timeEntryRepository.DeleteAsync(id);
            logger.LogInformation("Time entry deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting time entry with ID {Id}.", id);
            throw;
        }
    }
}