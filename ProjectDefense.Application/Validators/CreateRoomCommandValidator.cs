using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Room name is required.")
                .MaximumLength(100).WithMessage("Room name cannot exceed 100 characters.");

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Room number is required.")
                .MaximumLength(20).WithMessage("Room number cannot exceed 20 characters.");
        }
    }
}