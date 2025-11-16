using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers;

public class GetUserReservationsQueryHandler(IReservationRepository reservationRepository)
    : IRequestHandler<GetUserReservationsQuery, IEnumerable<ReservationDto>>
{
    public async Task<IEnumerable<ReservationDto>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = await reservationRepository.GetActiveReservationsByStudentIdAsync(request.UserId);
        return reservations.Select(r => new ReservationDto
        {
            Id = r.Id,
            RoomName = r.Availability?.Room?.Name ?? "-",
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            StudentName = r.Student?.UserName
        }).OrderBy(r => r.StartTime);
    }
}
