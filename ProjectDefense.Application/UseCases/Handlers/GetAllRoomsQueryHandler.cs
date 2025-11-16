using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers
{
    public class GetAllRoomsQueryHandler(IRoomRepository roomRepository)
        : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomDto>>
    {
        public async Task<IEnumerable<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await roomRepository.GetAllAsync();
            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Number = r.Number
            }).OrderBy(r => r.Name);
        }
    }
}