using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI
{
    public class ClockInOutDbContext : DbContext
    {
        public ClockInOutDbContext(DbContextOptions<ClockInOutDbContext> options) : base(options) { }

        public DbSet<User> UserEntries { get; set; }
    }
}