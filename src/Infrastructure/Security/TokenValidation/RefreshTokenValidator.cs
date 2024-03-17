using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
}