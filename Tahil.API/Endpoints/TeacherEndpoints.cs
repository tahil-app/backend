using Tahil.API.Authorization;
using Tahil.Application.Teachers.Commands;
using Tahil.Application.Teachers.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class TeacherEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var teachers = app.MapGroup("/teachers");

        #region Get Teachers

        teachers.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTeacherQuery(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.ViewDetail);

        teachers.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllTeachersQuery());
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.ViewAll);

        teachers.MapGet("/by-course/{courseId:int}", async (int courseId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCourseTeachersQueryQuery(courseId));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.ViewAll, "courseId", "course");

        teachers.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTeachersPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.ViewPaged);

        teachers.MapGet("/schedules/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTeacherSchedulesQuery(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.ViewDetail);

        teachers.MapGet("/groups/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTeacherGroupsQuery(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.ViewDetail);

        #endregion

        #region Create / Update / Delete

        teachers.MapPost("/create", async (TeacherDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateTeacherCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.Create);

        teachers.MapPut("/update", async (TeacherDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateTeacherCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.Update);

        teachers.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteTeacherCommand(id));
            return Results.Ok(user);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.Delete);

        #endregion

        #region Manage Activation

        teachers.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new ActivateTeacherCommand(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.Activate);

        teachers.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeActivateTeacherCommand(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.DeActivate);

        #endregion

        #region Manage Attachments

        teachers.MapPost("/upload-attachment", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadTeacherAttachmetCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery().RequireAccess(EntityType.Teacher, AuthorizationOperation.Update, "userId");

        teachers.MapDelete("/delete-attachment/{attachmentId:int}", async (int attachmentId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteTeacherAttachmentCommand(attachmentId));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Teacher, AuthorizationOperation.Delete, "attachmentId", "attachment");

        teachers.MapPost("/upload-image", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadTeacherImageCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery().RequireAccess(EntityType.Teacher, AuthorizationOperation.Update, "userId");

        #endregion

    }
}