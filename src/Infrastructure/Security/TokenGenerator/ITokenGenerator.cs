namespace Infrastructure.Security.TokenGenerator;

public interface ITokenGenerator
{
    string GenerateAccessToken(Guid userId, string email, string name, List<string> roles);

    string GenerateRefreshToken(Guid userId);
}
