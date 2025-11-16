using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class ChangeReservationCommandValidator : AbstractValidator<ChangeReservationCommand>
    {
        public ChangeReservationCommandValidator()
        {
            RuleFor(x => x.OldReservationId)
                .GreaterThan(0).WithMessage("A valid ID for the current reservation must be provided.");

            RuleFor(x => x.NewReservationId)
                .GreaterThan(0).WithMessage("A valid ID for the new reservation must be provided.");

            RuleFor(x => x.NewReservationId)
                .NotEqual(x => x.OldReservationId).WithMessage("The new reservation cannot be the same as the current one.");

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID must be provided.");
        }
    }
}