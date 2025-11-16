using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class BanStudentCommandValidator : AbstractValidator<BanStudentCommand>
    {
        public BanStudentCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID must be provided.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("A reason for the ban must be provided.")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");

            RuleFor(x => x.BanUntil)
                .GreaterThan(DateTimeOffset.UtcNow)
                .When(x => x.BanUntil.HasValue)
                .WithMessage("The ban expiration date must be in the future.");
        }
    }
}