using MediatR;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.UseCases.Queries
{
    public record GetAvailableSlotsQuery(DateTime? FromDate) : IRequest<IEnumerable<AvailableSlotDto>>;
}