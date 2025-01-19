using ClockAPI.Models;

namespace ClockAPI.Services;

public interface ITimeEntryService
{
    Task AddTimeEntryAsync(TimeEntry timeEntry);
    Task UpdateTimeEntryAsync(TimeEntry timeEntry);
    Task<TimeEntry> GetTimeEntryByIdAsync(int id);
    Task<List<TimeEntry>> GetAllTimeEntriesAsync();
    Task<List<TimeEntry>> GetTimeEntriesByUserIdAsync(int userId);
    Task DeleteTimeEntryAsync(int id);
}