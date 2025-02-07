using FluentValidation;
using TeamAPI.Models.DTOs;

namespace TeamAPI.Models;

/// <summary>
/// Validator for TeamDto.
/// </summary>
public class TeamDtoValidator : AbstractValidator<TeamDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamDtoValidator"/> class.
    /// </summary>
    public TeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MinimumLength(3).WithMessage("Team name must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Team name cannot exceed 100 characters.");
    }
}