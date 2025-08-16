using Tahil.API.Authorization;
using Tahil.Application.ClassSessions.Commands;
using Tahil.Application.ClassSessions.Queries;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class ClassSessionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var sessions = app.MapGroup("/sessions");

        #region Get Sessions

        sessions.MapPost("/user", async (SessionSearchCriteriaDto searchCriteriaDto, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserSessionsQuery(searchCriteriaDto));
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSession, AuthorizationOperation.ViewAll);

        sessions.MapGet("/lookups/{courseId:int}", async (int courseId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetClassSessionLookupQuery(courseId));
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSession, AuthorizationOperation.ViewAll);

        #endregion

        #region Refresh Sessions

        sessions.MapPost("/refresh", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new RefreshSessionsCommand());
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSession, AuthorizationOperation.ViewAll);

        #endregion

        #region Update / Status

        sessions.MapPut("/update", async (ClassSessionDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateClassSessionCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSession, AuthorizationOperation.Update);

        sessions.MapPut("/update-status/{id:int}/{status:int}", async (int id, ClassSessionStatus status , [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateClassSessionStatusCommand(id, status));
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSession, AuthorizationOperation.Update, metaData: "status");

        #endregion
    }
}