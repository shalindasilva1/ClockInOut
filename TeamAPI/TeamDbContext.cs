using Microsoft.EntityFrameworkCore;
using TeamAPI.Models;

namespace TeamAPI;

public class TeamDbContext : DbContext
{
    public TeamDbContext(DbContextOptions<TeamDbContext> options) : base(options)
    {
    }

    public DbSet<Team> TeamEntries { get; set; }
    public DbSet<TeamMember> TeamMemberEntries { get; set; }
}