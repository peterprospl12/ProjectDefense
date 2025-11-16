using MediatR;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Web.Api;

public static class SlotsEndpoints
{
    public static void MapSlotsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/slots/available", async (IMediator mediator) =>
            {
                var query = new GetAvailableSlotsQuery(null);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetAvailableSlots")
            .Produces<IEnumerable<AvailableSlotDto>>();

        app.MapGet("/api/rooms", async (IMediator mediator) =>
            {
                var query = new GetAllRoomsQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetRooms")
            .Produces<IEnumerable<RoomDto>>();

        app.MapPost("/api/slots/{id}/book", async (int id, BookReservationDto bookReservationDto, IMediator mediator) =>
            {
                var command = new BookReservationCommand(id, bookReservationDto.StudentIndex);
                await mediator.Send(command);
                return Results.Ok();
            })
            .WithName("BookSlot")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}