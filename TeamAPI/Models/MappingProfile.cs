using AutoMapper;
using TeamAPI.Models.DTOs;

namespace TeamAPI.Models;

/// <summary>
/// AutoMapper profile for mapping between Team and its DTOs.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        // Create mappings between Team and its DTOs
        CreateMap<TeamDto, Team>();
        CreateMap<Team, TeamDto>();
        CreateMap<Team, TeamDetailsDto>();
        CreateMap<TeamMember, TeamMemberDto>();
    }
}