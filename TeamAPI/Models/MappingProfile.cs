using AutoMapper;
using TeamAPI.Models.DTOs;

namespace TeamAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TeamDto, Team>();
        CreateMap<Team, TeamDto>();
        CreateMap<Team, TeamDetailsDto>();
        CreateMap<TeamMember, TeamMemberDto>();
    }
}