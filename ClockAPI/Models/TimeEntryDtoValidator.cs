using ClockAPI.Models.DTOs;
using FluentValidation;

namespace ClockAPI.Models;

public class TimeEntryDtoValidator : AbstractValidator<TimeEntryDto>
{
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