using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
    {
        public CancelReservationCommandValidator()
        {
            RuleFor(x => x.ReservationId)
                .GreaterThan(0).WithMessage("A valid Reservation ID must be provided.");

            RuleFor(x => x.CancelerId)
                .NotEmpty().WithMessage("The ID of the user canceling the reservation must be provided.");
        }
    }
}
