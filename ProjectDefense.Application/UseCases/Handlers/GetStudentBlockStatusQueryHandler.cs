using MediatR;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Application.UseCases.Handlers;

public class GetStudentBlockStatusQueryHandler(IStudentBlockRepository studentBlockRepository)
    : IRequestHandler<GetStudentBlockStatusQuery, bool>
{
    public async Task<bool> Handle(GetStudentBlockStatusQuery request, CancellationToken cancellationToken)
    {
        return await studentBlockRepository.IsStudentBannedAsync(request.StudentId);
    }
}
