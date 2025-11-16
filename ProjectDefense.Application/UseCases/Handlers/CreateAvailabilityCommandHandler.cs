using MediatR;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class CreateAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository)
        : IRequestHandler<CreateAvailabilityCommand>
    {
        public async Task Handle(CreateAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var fullStartDate = request.StartDate.ToDateTime(request.StartTime);
            var fullEndDate = request.EndDate.ToDateTime(request.EndTime);

            var hasConflict = await availabilityRepository.CheckForConflictsAsync(request.RoomId, fullStartDate, fullEndDate);
            if (hasConflict)
            {
                throw new InvalidOperationException("An availability for this room and time already exists.");
            }

            var availability = new LecturerAvailability
            {
                LecturerId = request.LecturerId,
                RoomId = request.RoomId,
                StartDate = fullStartDate,
                EndDate = fullEndDate,
                SlotDurationInMinutes = request.SlotDurationInMinutes,
                Reservations = []
            };

            for (var day = request.StartDate; day <= request.EndDate; day = day.AddDays(1))
            {
                for (var time = request.StartTime; time < request.EndTime; time = time.AddMinutes(request.SlotDurationInMinutes))
                {
                    var slotStart = day.ToDateTime(time);
                    var slotEnd = slotStart.AddMinutes(request.SlotDurationInMinutes);

                    if (slotEnd.TimeOfDay > request.EndTime.ToTimeSpan() && day == request.EndDate)
                    {
                        continue;
                    }

                    var reservation = new Reservation
                    {
                        Availability = availability,
                        StartTime = slotStart,
                        EndTime = slotEnd,
                        StudentId = null
                    };
                    availability.Reservations.Add(reservation);
                }
            }

            await availabilityRepository.AddAsync(availability);
        }
    }
}