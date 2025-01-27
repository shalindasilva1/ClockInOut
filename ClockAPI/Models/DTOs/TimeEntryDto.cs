namespace ClockAPI.Models.DTOs;

public record TimeEntryDto(
    int Id,
    int UserId,
    DateTime ClockInTime,
    DateTime? ClockOutTime,
    double? Latitude,
    double? Longitude);