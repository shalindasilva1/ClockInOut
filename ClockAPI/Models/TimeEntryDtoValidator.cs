using ClockAPI.Models.DTOs;
using FluentValidation;

namespace ClockAPI.Models;

/// <summary>
/// Validator for the TimeEntryDto class.
/// </summary>
public class TimeEntryDtoValidator : AbstractValidator<TimeEntryDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeEntryDtoValidator"/> class.
    /// </summary>
    public TimeEntryDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.ClockInTime).NotEmpty();

        RuleFor(x => x.ClockOutTime)
            .GreaterThanOrEqualTo(x => x.ClockInTime)
            .When(x => x.ClockOutTime.HasValue)
            .WithMessage("Clock-out time must be after clock-in time.");
    }
}