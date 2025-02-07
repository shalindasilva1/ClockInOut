using ClockAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClockAPI.Repositories;

/// <summary>
/// Repository for managing time entry data.
/// </summary>
public class TimeEntryRepository(ClockDbContext dbContext) : ITimeEntryRepository
{
   /// <summary>
   /// Retrieves a time entry by its ID.
   /// </summary>
   /// <param name="id">The ID of the time entry.</param>
   /// <returns>A task that represents the asynchronous operation. The task result contains the time entry.</returns>
   public async Task<TimeEntry> GetByIdAsync(int id)
   {
       return await dbContext.TimeEntries.FindAsync(id)
              ?? throw new Exception("Time entry not found");
   }

   /// <summary>
   /// Retrieves all time entries.
   /// </summary>
   /// <returns>A task that represents the asynchronous operation. The task result contains a list of all time entries.</returns>
   public async Task<List<TimeEntry>> GetAllAsync()
   {
       return await dbContext.TimeEntries.ToListAsync();
   }

   /// <summary>
   /// Retrieves time entries by user ID.
   /// </summary>
   /// <param name="userId">The ID of the user.</param>
   /// <returns>A task that represents the asynchronous operation. The task result contains a list of time entries for the specified user.</returns>
   public async Task<List<TimeEntry>> GetByUserIdAsync(int userId)
   {
       return await dbContext.TimeEntries
           .Where(t => t.UserId == userId)
           .ToListAsync();
   }

   /// <summary>
   /// Adds a new time entry.
   /// </summary>
   /// <param name="timeEntry">The time entry to add.</param>
   /// <returns>A task that represents the asynchronous operation.</returns>
   public async Task AddAsync(TimeEntry timeEntry)
   {
       dbContext.TimeEntries.Add(timeEntry);
       await dbContext.SaveChangesAsync();
   }

   /// <summary>
   /// Updates an existing time entry.
   /// </summary>
   /// <param name="timeEntry">The time entry to update.</param>
   /// <returns>A task that represents the asynchronous operation.</returns>
   public async Task UpdateAsync(TimeEntry timeEntry)
   {
       dbContext.TimeEntries.Update(timeEntry);
       await dbContext.SaveChangesAsync();
   }

   /// <summary>
   /// Deletes a time entry by its ID.
   /// </summary>
   /// <param name="id">The ID of the time entry to delete.</param>
   /// <returns>A task that represents the asynchronous operation.</returns>
   public async Task DeleteAsync(int id)
   {
       var timeEntry = await dbContext.TimeEntries.FindAsync(id);
       if (timeEntry != null)
       {
           dbContext.TimeEntries.Remove(timeEntry);
           await dbContext.SaveChangesAsync();
       }
   }
}