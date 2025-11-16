using MediatR;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class DeleteRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<DeleteRoomCommand>
    {
        public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetByIdAsync(request.Id);
            if (room == null)
            {
                return;
            }

            var isUsed = await roomRepository.IsRoomInUseAsync(request.Id);
            if (isUsed)
            {
                throw new InvalidOperationException("This room cannot be deleted because it is currently in use for scheduled reservations.");
            }

            await roomRepository.DeleteAsync(request.Id);
        }
    }
}