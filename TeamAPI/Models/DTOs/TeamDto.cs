namespace TeamAPI.Models.DTOs;

/// <summary>
/// Data transfer object for a team.
/// </summary>
public record TeamDto(int Id, string Name);

/// <summary>
/// Data transfer object for detailed team information.
/// </summary>
public record TeamDetailsDto(int Id, string Name, List<TeamMemberDto> TeamMembers);

/// <summary>
/// Data transfer object for a team member.
/// </summary>
public record TeamMemberDto(int UserId);