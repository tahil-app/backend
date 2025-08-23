using Tahil.API.Authorization;
using Tahil.Application.Students.Commands;
using Tahil.Application.Students.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;

namespace Tahil.API.Endpoints;

public class StudentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var students = app.MapGroup("/students");

        #region Get Students
        
        students.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStudentQuery(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.ViewDetail);

        students.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllStudentsQuery());
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.ViewAll);

        students.MapGet("/by-group/{groupId:int}", async (int groupId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetGroupStudentsQueryQuery(groupId));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.ViewDetail, "groupId", "group");

        students.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStudentsPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.ViewPaged);

        #endregion

        #region Create / Update / Delete

        students.MapPost("/create", async (StudentDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateStudentCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.Create);

        students.MapPut("/update", async (StudentDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateStudentCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.Update);

        students.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteStudentCommand(id));
            return Results.Ok(user);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.Delete);

        #endregion

        #region Manage Activation

        students.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new ActivateStudentCommand(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.Activate);

        students.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeActivateStudentCommand(id));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.DeActivate);

        #endregion

        #region Manage Attachments and Image

        students.MapPost("/upload-attachment", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadStudentAttachmetCommand(model));
            return Results.Ok(Result.Success(true));
        }).DisableAntiforgery().RequireAccess(EntityType.Student, AuthorizationOperation.Update, "userId");

        students.MapDelete("/delete-attachment/{attachmentId:int}", async (int attachmentId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteStudentAttachmentCommand(attachmentId));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.Delete, "attachmentId", "attachment");

        students.MapPost("/upload-image", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadStudentImageCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery().RequireAccess(EntityType.Student, AuthorizationOperation.Update, "userId");

        #endregion

        #region Manage Password

        students.MapPut("/update-password", async ([FromBody] UpdatePasswordParams model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateStudentPasswordCommand(model));
            return Results.Ok(result);
        }).RequireAccess(EntityType.Student, AuthorizationOperation.Update);

        #endregion

    }
}