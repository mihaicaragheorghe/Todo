namespace Application.Core;

public readonly record struct Error
{
    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public static Error Validation(
        string code = "Validation",
        string message = "A validation error occurred.") =>
            new(code, message, ErrorType.Validation);

    public static Error Unauthorized(
        string code = "Unauthorized",
        string message = "You are not authorized to perform this action.") =>
            new(code, message, ErrorType.Unauthorized);

    public static Error Forbidden(
        string code = "Forbidden",
        string message = "You are forbidden from performing this action.")
    {
        return new(code, message, ErrorType.Forbidden);
    }

    public static Error NotFound(
        string code = "NotFound",
        string message = "The requested resource was not found.") =>
            new(code, message, ErrorType.NotFound);

    public static Error Conflict(
        string code = "Conflict",
        string message = "A conflict occurred while processing the request.") =>
            new(code, message, ErrorType.Conflict);

    public static Error Failure(
        string code = "Failure",
        string message = "An error occurred while processing the request.") =>
            new(code, message, ErrorType.Failure);

    public static Error Internal(
        string code = "Server.Internal",
        string message = "An internal error occurred while processing the request.") =>
            new(code, message, ErrorType.Internal);

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }
}