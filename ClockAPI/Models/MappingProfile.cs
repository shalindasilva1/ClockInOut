using AutoMapper;
using ClockAPI.Models.DTOs;

namespace ClockAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TimeEntry, TimeEntryDto>();
        CreateMap<TimeEntryDto, TimeEntry>();
    }
}