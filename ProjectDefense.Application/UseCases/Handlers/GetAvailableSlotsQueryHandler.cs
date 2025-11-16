using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class GetAvailableSlotsQueryHandler(IReservationRepository reservationRepository)
        : IRequestHandler<GetAvailableSlotsQuery, IEnumerable<ReservationDto>>
    {
        public async Task<IEnumerable<ReservationDto>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            var availableSlots = await reservationRepository.GetAvailableSlotsAsync(request.FromDate);

            var reservationDtos = availableSlots.Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomName = r.Availability.Room.Name,
                RoomNumber = r.Availability.Room.Number,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                IsBlocked = r.IsBlocked,
            }).OrderBy(r => r.StartTime);

            return reservationDtos;
        }
    }
}