using System.Net;

using Application.Core;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    protected ActionResult Error(Error error)
    {
        return ErrorResult(error);
    }

    private ObjectResult ErrorResult(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
            ErrorType.Forbidden => HttpStatusCode.Forbidden,
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };

        return Problem(
            detail: error.Message,
            statusCode: (int)statusCode,
            title: error.Code);
    }
}