using Tahil.Application.Groups.Commands;
using Tahil.Application.Rooms.Commands;
using Tahil.Application.Rooms.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;

namespace Tahil.API.Endpoints;

public class RoomEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var rooms = app.MapGroup("/rooms");

        rooms.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllRoomsQuery());
            return Results.Ok(result);
        }).RequireAuthorization(Policies.ALL);

        rooms.MapPost("/paged", async ([FromBody]QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetRoomsPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        rooms.MapPost("/create", async (RoomDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateRoomCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        rooms.MapPut("/update", async (RoomDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateRoomCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        rooms.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new ActivateRoomCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);

        rooms.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeActivateRoomCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);

        rooms.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteRoomCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}