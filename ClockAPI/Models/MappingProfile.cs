using AutoMapper;
using ClockAPI.Models.DTOs;

namespace ClockAPI.Models;

/// <summary>
/// AutoMapper profile for mapping between TimeEntry and TimeEntryDto.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        // Create mappings between TimeEntry and TimeEntryDto
        CreateMap<TimeEntry, TimeEntryDto>();
        CreateMap<TimeEntryDto, TimeEntry>();
    }
}