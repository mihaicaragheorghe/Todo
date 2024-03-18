using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface IRefreshTokenValidator
{
    bool Validate(string token);

    ClaimsPrincipal? GetPrincipalFromToken(string token);

    string GetUserIdFromToken(string token);

    string GetSingleClaimValue(string token, string claimType);
}