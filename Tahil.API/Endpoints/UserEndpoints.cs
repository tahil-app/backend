//using Tahil.Application.Users.Models;
//using Tahil.Common.Contracts;
//using Tahil.Domain.Dtos;
//using Tahil.Domain.Enums;

//namespace Tahil.API.Endpoints;

//public class UserEndpoints : ICarterModule
//{
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        var user = app.MapGroup("/users");

//        user.MapGet("/{id:int}", async (int id, [FromServices] IMediator mediator) =>
//        {
//            var user = await mediator.Send(new GetUserQuery(id));
//            return Results.Ok(user);
//        }).RequireAuthorization(Policies.ALL);

//        user.MapGet("/{role:alpha}", async (string role, [FromServices] IMediator mediator) =>
//        {
//            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
//                return Results.BadRequest(Result<string>.Failure("Invalid user role."));

//            var users = await mediator.Send(new GetUsersQuery(parsedRole));
//            return Results.Ok(users);
//        }).RequireAuthorization(Policies.AdminOnly);

//        user.MapPost("/", async (UserDto model, [FromServices] IMediator mediator) =>
//        {
//            var result = await mediator.Send(new CreateUserCommand(model));
//            return Results.Ok(result);
//        }).RequireAuthorization(Policies.AdminOnly);

//        user.MapPut("/", async (UserDto model, [FromServices] IMediator mediator) =>
//        {
//            var result = await mediator.Send(new UpdateUserCommand(model));
//            return Results.Ok(result);
//        }).RequireAuthorization(Policies.ALL);

//        user.MapPut("/update-password", async (UpdatePasswordParams model, [FromServices] IMediator mediator) =>
//        {
//            var result = await mediator.Send(new UpdateUserPasswordCommand(model));
//            return Results.Ok(result);
//        }).RequireAuthorization(Policies.ALL);

//        user.MapPut("/activate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
//        {
//            var user = await mediator.Send(new ActivateUserCommand(id));
//            return Results.Ok(user);
//        }).RequireAuthorization(Policies.AdminOnly);

//        user.MapPut("/deactivate/{id:int}", async (int id, [FromServices] IMediator mediator) =>
//        {
//            var user = await mediator.Send(new DeActivateUserCommand(id));
//            return Results.Ok(user);
//        }).RequireAuthorization(Policies.AdminOnly);
//    }
//}