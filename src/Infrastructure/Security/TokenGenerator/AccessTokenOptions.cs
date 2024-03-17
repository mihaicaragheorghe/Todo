namespace Infrastructure.Security.TokenGenerator;

public record AccessTokenOptions(
    string Secret,
    string Issuer,
    string Audience,
    int ExpiresInMinutes);