using MediatR;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.UseCases.Queries
{
    public record GetAllRoomsQuery : IRequest<IEnumerable<RoomDto>>;
}