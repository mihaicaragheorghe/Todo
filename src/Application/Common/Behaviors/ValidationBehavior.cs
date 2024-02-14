using Application.Core;

using FluentValidation;

using MediatR;

namespace Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var firstFailure = validationResults
            .SelectMany(r => r.Errors)
            .FirstOrDefault(f => f != null);

        if (firstFailure is not null)
        {
            return (dynamic)Error.Validation(
                code: firstFailure.PropertyName,
                message: firstFailure.ErrorMessage);
        }

        return await next();
    }
}