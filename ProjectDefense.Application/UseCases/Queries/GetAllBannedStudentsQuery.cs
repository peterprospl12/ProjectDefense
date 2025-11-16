using MediatR;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.UseCases.Queries
{
    public record GetAllBannedStudentsQuery() : IRequest<IEnumerable<UserDto>>;
}
