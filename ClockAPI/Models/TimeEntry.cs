using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClockAPI.Models;

/// <summary>
/// Represents a time entry in the system.
/// </summary>
[Table("TimeEntries")]
public class TimeEntry
{
    /// <summary>
    /// Gets or sets the unique identifier for the time entry.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID associated with the time entry.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the clock-in time for the time entry.
    /// </summary>
    public DateTime ClockInTime { get; set; }

    /// <summary>
    /// Gets or sets the clock-out time for the time entry.
    /// </summary>
    public DateTime? ClockOutTime { get; set; }

    /// <summary>
    /// Gets or sets the latitude where the time entry was recorded.
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude where the time entry was recorded.
    /// </summary>
    public double? Longitude { get; set; }
}