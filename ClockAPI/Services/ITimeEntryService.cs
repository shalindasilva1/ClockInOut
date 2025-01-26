using ClockAPI.Models.DTOs;

namespace ClockAPI.Services;

public interface ITimeEntryService
{
    Task AddTimeEntryAsync(TimeEntryDto timeEntry);
    Task UpdateTimeEntryAsync(TimeEntryDto timeEntry);
    Task<TimeEntryDto> GetTimeEntryByIdAsync(int id);
    Task<List<TimeEntryDto>> GetAllTimeEntriesAsync();
    Task<List<TimeEntryDto>> GetTimeEntriesByUserIdAsync(int userId);
    Task DeleteTimeEntryAsync(int id);
}