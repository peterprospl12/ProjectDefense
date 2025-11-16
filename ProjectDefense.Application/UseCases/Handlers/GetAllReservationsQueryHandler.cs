using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class GetAllReservationsQueryHandler(IReservationRepository reservationRepository)
        : IRequestHandler<GetAllReservationsQuery, IEnumerable<ReservationDto>>
    {
        public async Task<IEnumerable<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = await reservationRepository.GetAllByLecturerIdAsync(request.LecturerId);

            var reservationDtos = reservations.Select(r => new ReservationDto
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
            }).OrderBy(r => r.StartTime);

            return reservationDtos;
        }
    }
}