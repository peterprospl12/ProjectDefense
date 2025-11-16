using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class CreateRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<CreateRoomCommand, RoomDto>
    {
        public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room
            {
                Name = request.Name,
                Number = request.Number
            };

            await roomRepository.AddAsync(room);

            return new RoomDto(id: room.Id, name: room.Name, number: room.Number);
        }
    }
}