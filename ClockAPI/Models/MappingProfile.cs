using ClockAPI.Models.DTOs;
using AutoMapper;
namespace ClockAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TimeEntry, TimeEntryDto>();
        CreateMap<TimeEntryDto, TimeEntry>();
    }
}