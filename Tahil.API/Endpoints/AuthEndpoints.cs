using Tahil.Application.Auth.Commands;
using Tahil.Application.Auth.Models;
using Tahil.Application.Auth.Queries;

namespace Tahil.API.Endpoints;

public class AuthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/auth");

        auth.MapPost("/login", async (LoginParams model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new LoginQuery(model));
            return Results.Ok(result);
        });

        auth.MapPost("/refresh-token", async ([FromBody] string token, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new RefreshTokenQuery(token));
            return Results.Ok(result);
        });

        auth.MapPost("/signup", async (SignupModel model, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new SignupCommand(model));
            return Results.Ok(result);
        });

        auth.MapPost("/forget-password/{email}", async (string email, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new ForgetPasswordCommand(email));
            return Results.Ok(result);
        });
    }
}