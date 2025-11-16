using MediatR;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record DeleteRoomCommand(int Id) : IRequest;
}