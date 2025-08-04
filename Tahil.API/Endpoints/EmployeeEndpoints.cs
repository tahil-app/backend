using Tahil.Application.Employees.Commands;
using Tahil.Application.Employees.Queries;
using Tahil.Common.Contracts;
using Tahil.Domain.Dtos;

namespace Tahil.API.Endpoints;

public class EmployeeEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var employees = app.MapGroup("/employees");

        employees.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetEmployeeQuery(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOrEmployee);

        employees.MapPost("/paged", async ([FromBody] QueryParams queryParams, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetEmployeesPagedQuery(queryParams));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOnly);

        employees.MapPost("/create", async (UserDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateEmployeeCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOnly);

        employees.MapPut("/update", async (UserDto model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateEmployeeCommand(model));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOnly);

        employees.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new ActivateEmployeeCommand(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOnly);

        employees.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new DeActivateEmployeeCommand(id));
            return Results.Ok(result);
        }).RequireAuthorization(Policies.AdminOnly);

        employees.MapPost("/upload-image", async ([FromForm] UserAttachmentModel model, [FromServices] IMediator mediator) =>
        {
            if (model.File == null || model.File.Length == 0)
                return Results.BadRequest("No file uploaded.");

            var result = await mediator.Send(new UploadEmployeeImageCommand(model));
            return Results.Ok(true);
        }).DisableAntiforgery().RequireAuthorization(Policies.AdminOnly);

        employees.MapDelete("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new DeleteEmployeeCommand(id));
            return Results.Ok(user);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}