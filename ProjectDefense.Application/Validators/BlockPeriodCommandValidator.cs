using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class BlockPeriodCommandValidator : AbstractValidator<BlockPeriodCommand>
    {
        public BlockPeriodCommandValidator()
        {
            RuleFor(x => x.LecturerId)
                .NotEmpty().WithMessage("Lecturer ID must be provided.");

            RuleFor(x => x.EndDateTime)
                .GreaterThan(x => x.StartDateTime)
                .WithMessage("End date and time must be after the start date and time.");
        }
    }
}