using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class BookReservationCommandValidator : AbstractValidator<BookReservationCommand>
    {
        public BookReservationCommandValidator()
        {
            RuleFor(x => x.ReservationId)
                .GreaterThan(0).WithMessage("A valid Reservation ID must be provided.");

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID cannot be empty.");
        }
    }
}