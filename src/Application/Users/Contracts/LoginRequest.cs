namespace Application.Users.Contracts;

public record LoginRequest(
    string Email,
    string Password);
