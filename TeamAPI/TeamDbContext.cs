using Microsoft.EntityFrameworkCore;
using TeamAPI.Models;

namespace TeamAPI;

/// <summary>
/// Database context for the Team API.
/// </summary>
public class TeamDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public TeamDbContext(DbContextOptions<TeamDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the Team entries.
    /// </summary>
    public DbSet<Team> TeamEntries { get; set; }

    /// <summary>
    /// Gets or sets the Team Member entries.
    /// </summary>
    public DbSet<TeamMember> TeamMemberEntries { get; set; }
}