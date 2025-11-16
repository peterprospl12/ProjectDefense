using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Role)
                .NotEmpty()
                .Must(role => role == "Student" || role == "Lecturer")
                .WithMessage("A valid role ('Student' or 'Lecturer') must be selected.");
        }
    }
}
