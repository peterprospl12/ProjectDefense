using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers;

public class GetAllReservationsByRangeQueryHandler(IReservationRepository reservationRepository) 
    : IRequestHandler<GetAllReservationsByRangeQuery, IEnumerable<ReservationDto>>
{
    public async Task<IEnumerable<ReservationDto>> Handle(GetAllReservationsByRangeQuery request, CancellationToken cancellationToken)
    {
        var reservations = await reservationRepository.GetReservationsForLecturerInPeriodAsync(
            request.LecturerId, 
            request.StartDate, 
            request.EndDate);

        return reservations
            .Where(r => r.Availability.RoomId == request.RoomId)
            .Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomName = r.Availability.Room.Name,
                RoomNumber = r.Availability.Room.Number,
                StudentName = r.Student?.UserName,
                StudentEmail = r.Student?.Email,   
                StartTime = r.StartTime,
                EndTime = r.EndTime,     
                StudentId = r.StudentId,
                IsBlocked = r.IsBlocked
            });
    }
}