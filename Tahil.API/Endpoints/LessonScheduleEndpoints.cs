using Tahil.Application.LessonSchedules.Queries;
using Tahil.Common.Contracts;

namespace Tahil.API.Endpoints;

public class LessonScheduleEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var schedules = app.MapGroup("/schedules");

        schedules.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLessonScheduleQuery(id));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        schedules.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLessonSchedulesPagedQuery(queryParams));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

    }
}