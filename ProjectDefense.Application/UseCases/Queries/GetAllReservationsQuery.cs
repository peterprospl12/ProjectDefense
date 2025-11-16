using MediatR;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.UseCases.Queries
{
    public record GetAllReservationsQuery(string LecturerId) : IRequest<IEnumerable<ReservationDto>>;
}