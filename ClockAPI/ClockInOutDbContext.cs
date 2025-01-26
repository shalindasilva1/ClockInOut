using ClockAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClockAPI;

public class ClockInOutDbContext : DbContext
{
    public ClockInOutDbContext(DbContextOptions<ClockInOutDbContext> options) : base(options)
    {
    }

    public DbSet<TimeEntry> TimeEntries { get; set; }
}