using Application.Core;

namespace Application.Common.Errors;

public static class UserErrors
{
    public static Error NotFound => Error.NotFound(
        code: "User.NotFound",
        message: "User not found.");

    public static Error Unauthorized => Error.Unauthorized(
        code: "User.Unauthorized",
        message: "You are not authorized to perform this action.");
}