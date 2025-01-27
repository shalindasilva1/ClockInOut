using ClockAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClockAPI.Repositories;

public class TimeEntryRepository(ClockDbContext dbContext) : ITimeEntryRepository
{
    private readonly ClockDbContext _dbContext = dbContext;

    public async Task<TimeEntry> GetByIdAsync(int id)
    {
        var result = await _dbContext.TimeEntries.FindAsync(id)
                     ?? throw new Exception("Time entry not found");
        return result;
    }

    public async Task<List<TimeEntry>> GetAllAsync()
    {
        return await _dbContext.TimeEntries.ToListAsync();
    }

    public async Task<List<TimeEntry>> GetByUserIdAsync(int userId)
    {
        return await _dbContext.TimeEntries
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(TimeEntry timeEntry)
    {
        _dbContext.TimeEntries.Add(timeEntry);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TimeEntry timeEntry)
    {
        _dbContext.TimeEntries.Update(timeEntry);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.TimeEntries
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync();
    }
}