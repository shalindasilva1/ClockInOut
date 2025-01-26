using Microsoft.EntityFrameworkCore;
using TeamAPI.Models;

namespace TeamAPI;

public class ClockInOutDbContext : DbContext
{
    public ClockInOutDbContext(DbContextOptions<ClockInOutDbContext> options) : base(options)
    {
    }

    public DbSet<Team> TeamEntries { get; set; }
    public DbSet<TeamMember> TeamMemberEntries { get; set; }
}