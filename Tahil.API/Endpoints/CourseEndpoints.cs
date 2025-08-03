using Tahil.Application.Courses.Commands;
using Tahil.Application.Courses.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;

namespace Tahil.API.Endpoints;

public class CourseEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var courses = app.MapGroup("/courses");

        courses.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllCoursesQuery());
            return Results.Ok(result);
        }).RequireAuthorization(Policies.ALL);

        courses.MapPost("/paged", async ([FromBody]QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCoursesPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        courses.MapPost("/create", async (CourseDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateCourseCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        courses.MapPut("/update", async (CourseDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateCourseCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        courses.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new ActivateCourseCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);

        courses.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeActivateCourseCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);

        courses.MapDelete("/delete/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteCourseCommand(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}