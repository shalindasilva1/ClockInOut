using ClockAPI.Models;
        using System.Collections.Generic;
        using System.Threading.Tasks;
        
        namespace ClockAPI.Repositories;
        
        /// <summary>
        /// Interface for managing time entry data.
        /// </summary>
        public interface ITimeEntryRepository
        {
            /// <summary>
            /// Retrieves a time entry by its ID.
            /// </summary>
            /// <param name="id">The ID of the time entry.</param>
            /// <returns>A task that represents the asynchronous operation. The task result contains the time entry.</returns>
            Task<TimeEntry> GetByIdAsync(int id);
        
            /// <summary>
            /// Retrieves all time entries.
            /// </summary>
            /// <returns>A task that represents the asynchronous operation. The task result contains a list of all time entries.</returns>
            Task<List<TimeEntry>> GetAllAsync();
        
            /// <summary>
            /// Retrieves time entries by user ID.
            /// </summary>
            /// <param name="userId">The ID of the user.</param>
            /// <returns>A task that represents the asynchronous operation. The task result contains a list of time entries for the specified user.</returns>
            Task<List<TimeEntry>> GetByUserIdAsync(int userId);
        
            /// <summary>
            /// Adds a new time entry.
            /// </summary>
            /// <param name="timeEntry">The time entry to add.</param>
            /// <returns>A task that represents the asynchronous operation.</returns>
            Task AddAsync(TimeEntry timeEntry);
        
            /// <summary>
            /// Updates an existing time entry.
            /// </summary>
            /// <param name="timeEntry">The time entry to update.</param>
            /// <returns>A task that represents the asynchronous operation.</returns>
            Task UpdateAsync(TimeEntry timeEntry);
        
            /// <summary>
            /// Deletes a time entry by its ID.
            /// </summary>
            /// <param name="id">The ID of the time entry to delete.</param>
            /// <returns>A task that represents the asynchronous operation.</returns>
            Task DeleteAsync(int id);
        }