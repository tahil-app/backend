using Tahil.Application.Students.Commands;
using Tahil.Application.Students.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;

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
        }).RequireAuthorization(Policies.ALL);

        students.MapGet("/all", async ([FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllStudentsQuery());
            return Results.Ok(result);
        }).RequireAuthorization(Policies.ALL);

        students.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStudentsPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployeeOrTeacher);

        #endregion

        #region Create / Update / Delete

        students.MapPost("/create", async (StudentDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateStudentCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        students.MapPut("/update", async (StudentDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateStudentCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        students.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteStudentCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);

        #endregion

        #region Manage Activation

        students.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new ActivateStudentCommand(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        students.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeActivateStudentCommand(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        #endregion

        #region Manage Attachments and Image

        students.MapPost("/upload-attachment", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadStudentAttachmetCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery().RequireAuthorization(Policies.AdminOrEmployee);

        students.MapDelete("/delete-attachment/{attachmentId:int}", async (int attachmentId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteStudentAttachmentCommand(attachmentId));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        students.MapPost("/upload-image", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadStudentImageCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery().RequireAuthorization(Policies.AdminOrEmployee);

        #endregion

    }
}