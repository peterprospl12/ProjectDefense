using MediatR;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class UpdateRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<UpdateRoomCommand>
    {
        public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetByIdAsync(request.Id);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {request.Id} not found.");
            }

            room.Name = request.Name;
            room.Number = request.Number;

            await roomRepository.UpdateAsync(room);
        }
    }
}