﻿namespace Tahil.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failuers =
            validationResults.Where(r => r.Errors.Any()).SelectMany(r => r.Errors).ToList();

        if (failuers.Any())
            throw new FluentValidation.ValidationException(failuers);

        return await next();
    }
}