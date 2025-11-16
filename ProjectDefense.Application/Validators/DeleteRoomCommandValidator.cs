using FluentValidation;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.Validators
{
    public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
    {
        public DeleteRoomCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid Room ID must be provided.");
        }
    }
}
