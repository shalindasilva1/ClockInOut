using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace TeamAPI.Models;

/// <summary>
/// Represents a team entity.
/// </summary>
public class Team
{
    /// <summary>
    /// Gets or sets the team ID.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the team name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of team members.
    /// </summary>
    public List<TeamMember> TeamMembers { get; set; } = new();
}

/// <summary>
/// Represents a team member entity.
/// </summary>
public class TeamMember
{
    /// <summary>
    /// Gets or sets the team member ID.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the team ID.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// Gets or sets the team.
    /// </summary>
    public Team Team { get; set; }

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int UserId { get; set; }
}