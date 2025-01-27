using ClockAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClockAPI;

public class ClockDbContext : DbContext
{
    public ClockDbContext(DbContextOptions<ClockDbContext> options) : base(options)
    {
    }

    public DbSet<TimeEntry> TimeEntries { get; set; }
}