using ClockAPI.Models;

namespace ClockAPI.Repositories;

public interface ITimeEntryRepository
{
    Task<TimeEntry> GetByIdAsync(int id);
    Task<List<TimeEntry>> GetAllAsync();
    Task<List<TimeEntry>> GetByUserIdAsync(int userId);
    Task AddAsync(TimeEntry timeEntry);
    Task UpdateAsync(TimeEntry timeEntry);
    Task DeleteAsync(int id);
}