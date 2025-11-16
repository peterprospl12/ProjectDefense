using MediatR;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers;

public class GetAllBannedStudentsQueryHandler(IStudentBlockRepository studentBlockRepository)
    : IRequestHandler<GetAllBannedStudentsQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetAllBannedStudentsQuery request, CancellationToken cancellationToken)
    {
        var bannedStudentIds = await studentBlockRepository.GetAllBannedStudentIdsAsync();
        return bannedStudentIds.Select(id => new UserDto { Id = id });
    }
}
