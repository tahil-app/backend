using Tahil.Application.Teachers.Commands;
using Tahil.Application.Teachers.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;

namespace Tahil.API.Endpoints;

public class TeacherEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var teachers = app.MapGroup("/teachers");

        teachers.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTeacherQuery(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployeeOrTeacher);

        teachers.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllTeachersQuery());
            return Results.Ok(result);
        }).RequireAuthorization(Policies.ALL);

        teachers.MapGet("/by-course/{courseId:int}", async (int courseId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCourseTeachersQueryQuery(courseId));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTeachersPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapPost("/create", async (TeacherDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateTeacherCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapPut("/update", async (TeacherDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateTeacherCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new ActivateTeacherCommand(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeActivateTeacherCommand(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapPost("/upload-attachment", async ([FromForm] UserAttachmentModel model,[FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadTeacherAttachmetCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery().RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapDelete("/delete-attachment/{attachmentId:int}", async (int attachmentId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteTeacherAttachmentCommand(attachmentId));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapPost("/upload-image", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadTeacherImageCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery().RequireAuthorization(Policies.AdminOrEmployee);

        teachers.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteTeacherCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}