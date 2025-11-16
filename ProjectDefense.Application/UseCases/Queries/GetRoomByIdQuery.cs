using MediatR;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.UseCases.Queries
{
    public class GetRoomByIdQuery : IRequest<RoomDto>
    {
        public int Id { get; set; }
    }
}
