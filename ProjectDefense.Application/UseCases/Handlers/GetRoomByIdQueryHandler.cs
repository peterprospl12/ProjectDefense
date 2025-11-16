using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class GetRoomByIdQueryHandler(IRoomRepository roomRepository) : IRequestHandler<GetRoomByIdQuery, RoomDto>
    {
        public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetByIdAsync(request.Id);

            if (room == null)
            {
                return null;
            }

            return new RoomDto(id: room.Id, name: room.Name, number: room.Number);
        }
    }
}