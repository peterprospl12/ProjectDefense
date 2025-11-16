using MediatR;

namespace ProjectDefense.Application.UseCases.Queries;

public record GetStudentBlockStatusQuery(string StudentId) : IRequest<bool>;