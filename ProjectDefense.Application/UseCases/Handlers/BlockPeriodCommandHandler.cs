using MediatR;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class BlockPeriodCommandHandler(IReservationRepository reservationRepository)
        : IRequestHandler<BlockPeriodCommand>
    {
        public async Task Handle(BlockPeriodCommand request, CancellationToken cancellationToken)
        {
            var reservationsToBlock = await reservationRepository.GetReservationsForLecturerInPeriodAsync(
                request.LecturerId,
                request.StartDateTime,
                request.EndDateTime);

            var toBlock = reservationsToBlock.ToList();
            if (!toBlock.Any())
            {
                return;
            }

            foreach (var reservation in toBlock)
            {
                reservation.StudentId = null;
                reservation.IsBlocked = true;
            }

            await reservationRepository.UpdateRangeAsync(toBlock);
        }
    }
}