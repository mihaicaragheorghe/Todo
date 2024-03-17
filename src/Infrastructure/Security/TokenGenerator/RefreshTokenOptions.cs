namespace Infrastructure.Security.TokenGenerator;

public record RefreshTokenOptions(
    string Secret,
    int ExpiresInHours);