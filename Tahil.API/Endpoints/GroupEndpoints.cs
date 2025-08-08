using Tahil.API.Authorization;
using Tahil.Application.Groups.Commands;
using Tahil.Application.Groups.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class GroupEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groups = app.MapGroup("/groups");

        #region Get Groups

        groups.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetGroupQuery(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Group, AuthorizationOperation.ViewDetail);

        groups.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllGroupsQuery());
            return Results.Ok(result);
        }).RequireAccess(EntityType.Group, AuthorizationOperation.ViewAll);

        groups.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetGroupsPagedQuery(queryParams));
            return Results.Ok(result);
        });//.RequireAccess(EntityType.Group, AuthorizationOperation.ViewPaged);

        #endregion

        #region Create / Update / Delete

        groups.MapPost("/create", async (GroupDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateGroupCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Group, AuthorizationOperation.Create);

        groups.MapPut("/update", async (GroupDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateGroupCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Group, AuthorizationOperation.Update);

        groups.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteGroupCommand(id));
            return Results.Ok(user);
        }).RequireAccess(EntityType.Group, AuthorizationOperation.Delete);

        #endregion

        #region Manage Group Students

        groups.MapPut("/update-students/{id:int}", async (int id, [FromBody] List<int> studentIds, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateGroupStudentsCommand(id, studentIds));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Group, AuthorizationOperation.Update);

        #endregion

    }
}