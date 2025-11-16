using MediatR;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record CreateRoomCommand(string Name, string Number) : IRequest<RoomDto>;
}