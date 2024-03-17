namespace Infrastructure.Security.TokenValidation;

public interface IRefreshTokenValidator
{
    bool Validate(string token);
}