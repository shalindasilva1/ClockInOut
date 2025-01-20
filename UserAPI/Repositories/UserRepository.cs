using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Repositories;

public class UserRepository(ClockInOutDbContext dbContext) : IUserRepository
{
    private readonly ClockInOutDbContext _dbContext = dbContext;
    
    public async Task<User> GetByIdAsync(int id)
    {
        var result = await _dbContext.UserEntries.FindAsync(id) 
                     ?? throw new Exception("User entry not found");
        return result;
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var result = await _dbContext.UserEntries
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync()
            ?? throw new Exception("User entry not found");
        return result;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _dbContext.UserEntries.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        _dbContext.UserEntries.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.UserEntries.Update(user); 
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.UserEntries
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync();
    }
}