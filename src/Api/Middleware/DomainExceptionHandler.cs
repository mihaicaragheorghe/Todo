using System.Net;

using Domain.Common;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public class DomainExceptionHandler(ILogger<DomainException> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DomainException domainException)
        {
            return false;
        }

        logger.LogError(exception, "A domain exception occurred.");

        var problemDetails = new ProblemDetails
        {
            Title = "Domain.RuleViolation",
            Detail = domainException.Message,
            Status = (int)HttpStatusCode.BadRequest
        };

        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken: cancellationToken);

        return true;
    }
}
