using Application.Core;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Starting request {Request}", requestName);

        var result = await next();

        if (!result.IsSuccessful)
        {
            logger.LogWarning("Request {Request} failed with error {@Error}", requestName, result.Error);
        }

        logger.LogInformation("Completed request {Request}", requestName);

        return result;
    }
}