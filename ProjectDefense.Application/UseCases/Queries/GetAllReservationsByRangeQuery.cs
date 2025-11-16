using MediatR;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.UseCases.Queries;

public record GetAllReservationsByRangeQuery(
    string LecturerId, 
    int RoomId, 
    DateTime StartDate, 
    DateTime EndDate) : IRequest<IEnumerable<ReservationDto>>;