using Application.Core;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public class ApplicationExceptionHandler(ILogger<ServiceException> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ServiceException serviceException)
        {
            logger.LogError(serviceException, "An application exception occurred.");

            var error = serviceException.Error;
            var problemDetails = new ProblemDetails
            {
                Title = error.Code,
                Detail = error.Message,
                Status = (int)error.StatusCode,
            };

            httpContext.Response.StatusCode = (int)error.StatusCode;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        return false;
    }
}