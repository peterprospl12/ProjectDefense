using MediatR;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record CreateAvailabilityCommand(string LecturerId, int RoomId, DateOnly StartDate, DateOnly EndDate, TimeOnly StartTime, TimeOnly EndTime, int SlotDurationInMinutes) : IRequest;
}
