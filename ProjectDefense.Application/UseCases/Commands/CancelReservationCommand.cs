using MediatR;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record CancelReservationCommand(int ReservationId, string CancelerId) : IRequest;
}
