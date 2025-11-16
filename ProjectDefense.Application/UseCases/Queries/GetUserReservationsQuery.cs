using MediatR;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.UseCases.Queries
{
    public record GetUserReservationsQuery(string UserId) : IRequest<IEnumerable<ReservationDto>>;
}

