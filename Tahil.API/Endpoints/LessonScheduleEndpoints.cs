using Tahil.Application.LessonSchedules.Commands;
using Tahil.Application.LessonSchedules.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;

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
        
        schedules.MapGet("/lookups", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLessonScheduleLookupQuery());
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        schedules.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLessonSchedulesPagedQuery(queryParams));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        schedules.MapPost("/create", async (LessonScheduleDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateLessonScheduleCommand(model));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        schedules.MapPut("/update", async (LessonScheduleDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateLessonScheduleCommand(model));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

    }
}