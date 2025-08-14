using Tahil.API.Authorization;
using Tahil.Application.ClassSessions.Commands;
using Tahil.Application.ClassSessions.Queries;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class ClassSessionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var sessions = app.MapGroup("/sessions");

        #region Get Sessions

        sessions.MapGet("/user", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserSessionsQuery());
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
    }
}