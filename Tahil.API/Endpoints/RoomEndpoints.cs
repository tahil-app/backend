using Tahil.API.Authorization;
using Tahil.Application.Rooms.Commands;
using Tahil.Application.Rooms.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class RoomEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var rooms = app.MapGroup("/rooms");

        #region Get Groups

        rooms.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllRoomsQuery());
            return Results.Ok(result);
        }).RequireAccess(EntityType.Room, AuthorizationOperation.ViewDetail);

        rooms.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetRoomsPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Room, AuthorizationOperation.ViewPaged);

        #endregion

        #region Create / Update / Delete

        rooms.MapPost("/create", async (RoomDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateRoomCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Room, AuthorizationOperation.Create);

        rooms.MapPut("/update", async (RoomDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateRoomCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Room, AuthorizationOperation.Update);

        rooms.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteRoomCommand(id));
            return Results.Ok(user);
        }).RequireAccess(EntityType.Room, AuthorizationOperation.Delete);

        #endregion

        #region Manage Activation

        rooms.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new ActivateRoomCommand(id));
            return Results.Ok(user);
        }).RequireAccess(EntityType.Room, AuthorizationOperation.Activate);

        rooms.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeActivateRoomCommand(id));
            return Results.Ok(user);
        }).RequireAccess(EntityType.Room, AuthorizationOperation.DeActivate);

        #endregion

    }
}