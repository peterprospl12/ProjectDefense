using MediatR;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record BookReservationCommand(int ReservationId, string StudentId) : IRequest;
}
