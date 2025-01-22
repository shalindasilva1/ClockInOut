using AutoMapper;
using ClockAPI.Models;
using ClockAPI.Models.DTOs;
using ClockAPI.Repositories;

namespace ClockAPI.Services;

public class TimeEntryService(
    ITimeEntryRepository timeEntryRepository,
    IMapper mapper) : ITimeEntryService
{
    public async Task AddTimeEntryAsync(TimeEntryDto timeEntry)
    {
        await timeEntryRepository.AddAsync(mapper.Map<TimeEntry>(timeEntry));
    }

    public async Task UpdateTimeEntryAsync(TimeEntryDto timeEntry)
    {
        await timeEntryRepository.UpdateAsync(mapper.Map<TimeEntry>(timeEntry));
    }

    public async Task<TimeEntryDto> GetTimeEntryByIdAsync(int id)
    {
        var result = await timeEntryRepository.GetByIdAsync(id);
        return mapper.Map<TimeEntryDto>(result);
    }

    public async Task<List<TimeEntryDto>> GetAllTimeEntriesAsync()
    {
        var result = await timeEntryRepository.GetAllAsync();
        return mapper.Map<List<TimeEntryDto>>(result);
    }

    public async Task<List<TimeEntryDto>> GetTimeEntriesByUserIdAsync(int userId)
    {
        var result = await timeEntryRepository.GetByUserIdAsync(userId);
        return mapper.Map<List<TimeEntryDto>>(result);
    }

    public async Task DeleteTimeEntryAsync(int id)
    {
        await timeEntryRepository.DeleteAsync(id);
    }
}

