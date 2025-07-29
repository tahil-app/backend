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
        });//.RequireAuthorization(Policies.AdminOnly);

        teachers.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetTeachersPagedQuery(queryParams));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        teachers.MapPost("/create", async (TeacherDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateTeacherCommand(model));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        teachers.MapPut("/update", async (TeacherDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateTeacherCommand(model));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.ALL);

        teachers.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new ActivateTeacherCommand(id));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        teachers.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeActivateTeacherCommand(id));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        teachers.MapPost("/upload-attachment", async ([FromForm] UserAttachmentModel model,[FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadTeacherAttachmetCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery(); //.RequireAuthorization(Policies.AdminOnly);

        teachers.MapDelete("/delete-attachment/{attachmentId:int}", async (int attachmentId, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteTeacherAttachmentCommand(attachmentId));
            return Results.Ok(result);
        });//.RequireAuthorization(Policies.AdminOnly);

        teachers.MapPost("/upload-image", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadTeacherImageCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery(); //.RequireAuthorization(Policies.AdminOnly);
    }
}