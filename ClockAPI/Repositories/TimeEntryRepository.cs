using ClockAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClockAPI.Repositories;

/// <summary>
/// Repository for managing time entry data.
/// </summary>
public class TimeEntryRepository(ClockDbContext dbContext, ILogger<TimeEntryRepository> logger) : ITimeEntryRepository
{
    /// <inheritdoc />
    public async Task<TimeEntry> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Retrieving time entry with ID {Id}.", id);
            var timeEntry = await dbContext.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                logger.LogWarning("Time entry with ID {Id} not found.", id);
                throw new KeyNotFoundException("Time entry not found.");
            }
            logger.LogInformation("Time entry with ID {Id} retrieved successfully.", id);
            return timeEntry;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving time entry with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<TimeEntry>> GetAllAsync()
    {
        try
        {
            logger.LogInformation("Retrieving all time entries.");
            var timeEntries = await dbContext.TimeEntries.ToListAsync();
            logger.LogInformation("All time entries retrieved successfully.");
            return timeEntries;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all time entries.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<TimeEntry>> GetByUserIdAsync(int userId)
    {
        try
        {
            logger.LogInformation("Retrieving time entries for user ID {UserId}.", userId);
            var timeEntries = await dbContext.TimeEntries
                .Where(t => t.UserId == userId)
                .ToListAsync();
            logger.LogInformation("Time entries for user ID {UserId} retrieved successfully.", userId);
            return timeEntries;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving time entries for user ID {UserId}.", userId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task AddAsync(TimeEntry timeEntry)
    {
        try
        {
            logger.LogInformation("Adding a new time entry.");
            await dbContext.TimeEntries.AddAsync(timeEntry);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Time entry added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding time entry.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TimeEntry timeEntry)
    {
        try
        {
            logger.LogInformation("Updating time entry with ID {Id}.", timeEntry.Id);
            dbContext.TimeEntries.Update(timeEntry);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Time entry with ID {Id} updated successfully.", timeEntry.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating time entry with ID {Id}.", timeEntry.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        try
        {
            logger.LogInformation("Deleting time entry with ID {Id}.", id);
            var timeEntry = await dbContext.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                logger.LogWarning("Time entry with ID {Id} not found.", id);
                throw new KeyNotFoundException("Time entry not found.");
            }
            dbContext.TimeEntries.Remove(timeEntry);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Time entry with ID {Id} deleted successfully.", id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting time entry with ID {Id}.", id);
            throw;
        }
    }
}