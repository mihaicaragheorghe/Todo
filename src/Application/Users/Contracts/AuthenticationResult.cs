namespace Application.Users.Contracts;

public record AuthenticationResult(
    Guid UserId,
    string Email,
    string Name,
    string AccessToken,
    string RefreshToken);