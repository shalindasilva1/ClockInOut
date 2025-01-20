namespace ClockAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TimeEntries")]
public class TimeEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime ClockInTime { get; set; }

    public DateTime? ClockOutTime { get; set; } // Nullable for when clocking in

    public double? Latitude { get; set; } // Optional

    public double? Longitude { get; set; } // Optional
}