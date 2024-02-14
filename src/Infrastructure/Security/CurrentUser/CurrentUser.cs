namespace Infrastructure.Security.CurrentUser;

public record CurrentUser(
    Guid UserId,
    string Email,
    string Name,
    IReadOnlyList<string> Roles);