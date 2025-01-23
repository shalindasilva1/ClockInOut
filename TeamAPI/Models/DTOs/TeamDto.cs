namespace TeamAPI.Models.DTOs;

public record TeamDto(string Name); 
public record TeamDetailsDto(int Id, string Name, List<TeamMemberDto> TeamMembers);
public record TeamMemberDto(int UserId);