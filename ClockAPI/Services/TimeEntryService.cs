using ClockAPI.Models;
using ClockAPI.Repositories;

namespace ClockAPI.Services;

public class TimeEntryService(ITimeEntryRepository timeEntryRepository) : ITimeEntryService
{
    public async Task AddTimeEntryAsync(TimeEntry timeEntry)
    {
        await timeEntryRepository.AddAsync(timeEntry);
    }

    public async Task UpdateTimeEntryAsync(TimeEntry timeEntry)
    {
        await timeEntryRepository.UpdateAsync(timeEntry);
    }

    public async Task<TimeEntry> GetTimeEntryByIdAsync(int id)
    {
        return await timeEntryRepository.GetByIdAsync(id);
    }

    public async Task<List<TimeEntry>> GetAllTimeEntriesAsync()
    {
        return await timeEntryRepository.GetAllAsync();
    }

    public async Task<List<TimeEntry>> GetTimeEntriesByUserIdAsync(int userId)
    {
        return await timeEntryRepository.GetByUserIdAsync(userId);
    }

    public async Task DeleteTimeEntryAsync(int id)
    {
        await timeEntryRepository.DeleteAsync(id);
    }
}

