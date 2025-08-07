using Tahil.API.Authorization;
using Tahil.Application.Courses.Commands;
using Tahil.Application.Courses.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class CourseEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var courses = app.MapGroup("/courses");

        #region Get Courses

        courses.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCourseQuery(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.ViewDetail);

        courses.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllCoursesQuery());
            return Results.Ok(result);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.ViewAll);

        courses.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCoursesPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.ViewPaged);

        #endregion

        #region Create / Update / Delete

        courses.MapPost("/create", async (CourseDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateCourseCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.Create);

        courses.MapPut("/update", async (CourseDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateCourseCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.UpdateWithEntity);

        courses.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteCourseCommand(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.Delete);

        #endregion

        #region Manage Activation

        courses.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new ActivateCourseCommand(id));
            return Results.Ok(user);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.Activate);

        courses.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeActivateCourseCommand(id));
            return Results.Ok(user);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.DeActivate);

        #endregion

        #region Manage Course Teacher

        courses.MapPut("/update-teachers/{id:int}", async (int id, [FromBody] List<int> teacherIds, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateCourseTeachersCommand(id, teacherIds));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Course, AuthorizationOperation.UpdateWithId);

        #endregion

    }
}