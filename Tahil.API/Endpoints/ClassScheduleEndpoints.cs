using Tahil.Application.ClassSchedules.Queries;
using Tahil.Application.ClassSchedules.Commands;
using Tahil.Domain.Dtos;
using Tahil.API.Authorization;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class ClassScheduleEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var schedules = app.MapGroup("/schedules");

        #region Get Schedules

        schedules.MapGet("/lookups", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetClassScheduleLookupQuery());
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSchedule, AuthorizationOperation.ViewAll);

        schedules.MapGet("monthly/{month:int}/{year:int}", async (int month, int year, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetMonthlyClassSchedulesQuery(month, year));
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSchedule, AuthorizationOperation.ViewAll);

        #endregion

        #region Create / Update / Delete

        schedules.MapPost("/create", async (ClassScheduleDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateClassScheduleCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSchedule, AuthorizationOperation.Create);

        schedules.MapPut("/update", async (ClassScheduleDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateClassScheduleCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSchedule, AuthorizationOperation.Update);

        schedules.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteClassScheduleCommand(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.ClassSchedule, AuthorizationOperation.Delete);

        #endregion

    }
}