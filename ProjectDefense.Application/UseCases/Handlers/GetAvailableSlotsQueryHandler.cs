using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class GetAvailableSlotsQueryHandler(IReservationRepository reservationRepository)
        : IRequestHandler<GetAvailableSlotsQuery, IEnumerable<AvailableReservationDto>>
    {
        public async Task<IEnumerable<AvailableReservationDto>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            var availableSlots = await reservationRepository.GetAvailableSlotsAsync(request.FromDate);

            var reservationDtos = availableSlots.Select(r => new AvailableReservationDto
            {
                Id = r.Id,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                RoomName = r.Availability.Room.Name,
                RoomNumber = r.Availability.Room.Number,
            }).OrderBy(r => r.StartTime);

            return reservationDtos;
        }
    }
}