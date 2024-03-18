using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application.Common.Interfaces;

using Infrastructure.Security.TokenGenerator;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security.TokenValidation;

public class RefreshTokenValidator(
    IOptions<RefreshTokenOptions> refreshTokenOptions,
    ILogger<RefreshTokenValidator> logger)
        : IRefreshTokenValidator
{
    private readonly RefreshTokenOptions _refreshTokenOptions = refreshTokenOptions.Value;

    public bool Validate(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_refreshTokenOptions.Secret)),
            };

            tokenHandler.ValidateToken(token, validationParameters, out _);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating refresh token");

            return false;
        }
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_refreshTokenOptions.Secret)),
                },
                out var securityToken);
            
            if (IsJwtWithValidSecurityAlgorithm(securityToken))
            {
                return principal;
            }

            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting principal from refresh token");

            return null;
        }
    }

    public string GetUserIdFromToken(string token)
    {
        return GetSingleClaimValue(token, ClaimTypes.NameIdentifier);
    }

    public string GetSingleClaimValue(string token, string claimType)
    {
        var principal = GetPrincipalFromToken(token);

        return principal?.Claims.Single(c => c.Type == claimType)?.Value
            ?? throw new InvalidOperationException("Claim not found");
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken securityToken)
    {
        return (securityToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(
                   SecurityAlgorithms.HmacSha512,
                   StringComparison.InvariantCultureIgnoreCase);
    }
}