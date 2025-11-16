using MediatR;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;
using System.Security;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class ChangeReservationCommandHandler(IReservationRepository reservationRepository)
        : IRequestHandler<ChangeReservationCommand>
    {
        public async Task Handle(ChangeReservationCommand request, CancellationToken cancellationToken)
        {
            var reservations = (await reservationRepository.GetByIdsAsync(request.OldReservationId, request.NewReservationId)).ToList();

            var oldReservation = reservations.FirstOrDefault(r => r.Id == request.OldReservationId);
            var newReservation = reservations.FirstOrDefault(r => r.Id == request.NewReservationId);

            if (oldReservation == null || newReservation == null)
            {
                throw new KeyNotFoundException("One or both of the specified reservation slots could not be found.");
            }

            if (oldReservation.StudentId != request.StudentId)
            {
                throw new SecurityException("You are not authorized to change this reservation.");
            }

            if (newReservation.StudentId != null)
            {
                throw new InvalidOperationException("The selected new slot is no longer available.");
            }
            if (newReservation.IsBlocked)
            {
                throw new InvalidOperationException("The selected new slot is currently blocked by the lecturer.");
            }

            if (oldReservation.StartTime <= DateTime.UtcNow || newReservation.StartTime <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot change a reservation that is in the past.");
            }

            oldReservation.StudentId = null;

            newReservation.StudentId = request.StudentId;

            await reservationRepository.UpdateRangeAsync(new[] { oldReservation, newReservation });
        }
    }
}