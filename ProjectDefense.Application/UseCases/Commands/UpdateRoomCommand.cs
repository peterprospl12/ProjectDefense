using MediatR;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record UpdateRoomCommand(int Id, string Name, string Number) : IRequest;
}