using Microsoft.EntityFrameworkCore;
using ClockAPI.Models;

namespace ClockAPI
{
    public class ClockInOutDbContext : DbContext
    {
        public ClockInOutDbContext(DbContextOptions<ClockInOutDbContext> options) : base(options) { }

        public DbSet<TimeEntry> TimeEntries { get; set; }
    }
}