using Tahil.Application.ClassSchedules.Queries;
using Tahil.Application.ClassSchedules.Commands;
using Tahil.Domain.Dtos;

namespace Tahil.API.Endpoints;

public class ClassScheduleEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var schedules = app.MapGroup("/schedules");

        //schedules.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        //{
        //    var result = await mediator.Send(new GetClassScheduleQuery(id));
        //    return Results.Ok(result);
        //});//.RequireAuthorization(Policies.AdminOnly);

        schedules.MapGet("/lookups", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetClassScheduleLookupQuery());
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        //schedules.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        //{
        //    var result = await mediator.Send(new GetClassSchedulesPagedQuery(queryParams));
        //    return Results.Ok(result);
        //});//.RequireAuthorization(Policies.AdminOnly);

        schedules.MapPost("/create", async (ClassScheduleDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateClassScheduleCommand(model));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        //schedules.MapPut("/update", async (ClassScheduleDto model, [FromServices] IMediator mediator) =>
        //{
        //    var result = await mediator.Send(new UpdateClassScheduleCommand(model));
        //    return Results.Ok(result);
        //});//.RequireAuthorization(Policies.AdminOnly);

    }
}