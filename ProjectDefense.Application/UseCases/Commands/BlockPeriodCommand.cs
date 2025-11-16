using MediatR;

namespace ProjectDefense.Application.UseCases.Commands
{
    public record BlockPeriodCommand(
        string LecturerId,
        DateTime StartDateTime,
        DateTime EndDateTime) : IRequest;
}