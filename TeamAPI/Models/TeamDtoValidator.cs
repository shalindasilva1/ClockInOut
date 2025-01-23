using FluentValidation;
using TeamAPI.Models.DTOs;

namespace TeamAPI.Models;

public class TeamDtoValidator : AbstractValidator<TeamDto>
{
    public TeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MinimumLength(3).WithMessage("Team name must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Team name cannot exceed 100 characters.");
    }
}