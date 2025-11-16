using MediatR;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record BanStudentCommand(string StudentId, string Reason, DateTimeOffset? BanUntil) : IRequest;
}