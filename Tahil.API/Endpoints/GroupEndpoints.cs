using Tahil.Application.Groups.Commands;
using Tahil.Application.Groups.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;

namespace Tahil.API.Endpoints;

public class GroupEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groups = app.MapGroup("/groups");

        groups.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetGroupQuery(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployeeOrTeacher);

        groups.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllGroupsQuery());
            return Results.Ok(result);
        }).RequireAuthorization(Policies.ALL);

        groups.MapPost("/paged", async ([FromBody]QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetGroupsPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployeeOrTeacher);

        groups.MapPost("/create", async (GroupDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateGroupCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        groups.MapPut("/update", async (GroupDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateGroupCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        groups.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteGroupCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}