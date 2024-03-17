using System.Text;

using Infrastructure.Security.TokenGenerator;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security.TokenValidation;

public class AccessTokenValidationConfiguration(IOptions<AccessTokenOptions> accessTokenOptions)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AccessTokenOptions _accessTokenOptions = accessTokenOptions.Value;

    public void Configure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _accessTokenOptions.Issuer,
            ValidAudience = _accessTokenOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_accessTokenOptions.Secret)),
        };
    }

    public void Configure(JwtBearerOptions options) => Configure(Options.DefaultName, options);
}