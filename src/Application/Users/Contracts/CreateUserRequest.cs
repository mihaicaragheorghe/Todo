namespace Application.Users.Contracts;

public record CreateUserRequest(string Email, string Name, string Password);