using ClockAPI.Models.DTOs;
    
    namespace ClockAPI.Services;
    
    /// <summary>
    /// Interface for TimeEntry service operations.
    /// </summary>
    public interface ITimeEntryService
    {
        /// <summary>
        /// Adds a new time entry asynchronously.
        /// </summary>
        /// <param name="timeEntry">The time entry DTO to add.</param>
        Task AddTimeEntryAsync(TimeEntryDto timeEntry);
    
        /// <summary>
        /// Updates an existing time entry asynchronously.
        /// </summary>
        /// <param name="timeEntry">The time entry DTO to update.</param>
        Task UpdateTimeEntryAsync(TimeEntryDto timeEntry);
    
        /// <summary>
        /// Retrieves a time entry by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the time entry to retrieve.</param>
        /// <returns>The time entry DTO.</returns>
        Task<TimeEntryDto> GetTimeEntryByIdAsync(int id);
    
        /// <summary>
        /// Retrieves all time entries asynchronously.
        /// </summary>
        /// <returns>A list of all time entry DTOs.</returns>
        Task<List<TimeEntryDto>> GetAllTimeEntriesAsync();
    
        /// <summary>
        /// Retrieves time entries by user ID asynchronously.
        /// </summary>
        /// <param name="userId">The user ID to filter time entries.</param>
        /// <returns>A list of time entry DTOs for the specified user.</returns>
        Task<List<TimeEntryDto>> GetTimeEntriesByUserIdAsync(int userId);
    
        /// <summary>
        /// Deletes a time entry by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the time entry to delete.</param>
        Task DeleteTimeEntryAsync(int id);
    }