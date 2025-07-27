using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Tahil.Common.Contracts;
using Tahil.Common.Exceptions;

namespace Tahil.API.Middlewares;

public class CustomExceptionHandling(ILogger<CustomExceptionHandling> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        (string Detail, string Title, int StatusCode) = exception switch
        {
            NotFoundException =>
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status404NotFound
            ),
            ValidationException =>
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            InvalidOperationException =>
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            ),
            _ =>
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            )
        };

        context.Response.StatusCode = StatusCode;

        var problemDetails = new ProblemDetails
        {
            Type = Title,
            Detail = Detail,
            Status = StatusCode
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        var result = Result<ProblemDetails>.Failure(problemDetails, Detail);

        await context.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
        return true;
    }
}