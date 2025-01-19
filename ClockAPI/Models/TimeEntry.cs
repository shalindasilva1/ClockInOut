namespace ClockAPI.Models;

public class TimeEntry
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime ClockInTime { get; set; }
    public DateTime? ClockOutTime { get; set; } // Nullable for when clocking in
    public double? Latitude { get; set; } // Optional
    public double? Longitude { get; set; } // Optional
}