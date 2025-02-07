namespace ClockAPI.Models.DTOs;
    
    /// <summary>
    /// Data Transfer Object for TimeEntry.
    /// </summary>
    public record TimeEntryDto(
        int Id,
        int UserId,
        DateTime ClockInTime,
        DateTime? ClockOutTime,
        double? Latitude,
        double? Longitude);