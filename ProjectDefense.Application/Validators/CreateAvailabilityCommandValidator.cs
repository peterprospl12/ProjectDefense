using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class CreateAvailabilityCommandValidator : AbstractValidator<CreateAvailabilityCommand>
    {
        public CreateAvailabilityCommandValidator()
        {
            RuleFor(x => x.LecturerId)
                .NotEmpty().WithMessage("Lecturer ID must be specified.");

            RuleFor(x => x.RoomId)
                .GreaterThan(0).WithMessage("A valid Room ID must be specified.");

            RuleFor(x => x.SlotDurationInMinutes)
                .GreaterThan(0).WithMessage("Slot duration must be greater than 0 minutes.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be on or after the start date.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after the start time.");
        }
    }
}