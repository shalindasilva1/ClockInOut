namespace TeamAPI.Models.DTOs;

public record TeamDto(int Id, string Name);

public record TeamDetailsDto(int Id, string Name, List<TeamMemberDto> TeamMembers);

public record TeamMemberDto(int UserId);