using ClockAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClockAPI;

/// <summary>
/// Database context for the Clock API.
/// </summary>
public class ClockDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClockDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public ClockDbContext(DbContextOptions<ClockDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the Time entries.
    /// </summary>
    public DbSet<TimeEntry> TimeEntries { get; set; }
}