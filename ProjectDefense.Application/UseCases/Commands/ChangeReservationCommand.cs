using MediatR;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record ChangeReservationCommand(int OldReservationId, int NewReservationId, string StudentId) : IRequest;
}