using System.Text;

using Api.Middleware;

using Domain.Users;

using Infrastructure.Common;
using Infrastructure.Security.TokenGenerator;
using Infrastructure.Security.TokenValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddExceptionHandler<DomainExceptionHandler>();
        services.AddExceptionHandler<ApplicationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddUserStore<AppDbContext>();

        var accessTokenSection = configuration.GetSection("AccessToken");

        services.AddOptions<AccessTokenOptions>()
            .Bind(accessTokenSection)
            .Validate(opts => !string.IsNullOrWhiteSpace(opts.Secret) 
                && opts.ExpiresInMinutes > 0
                && !string.IsNullOrWhiteSpace(opts.Issuer)
                && !string.IsNullOrWhiteSpace(opts.Audience)) 
            .ValidateOnStart();

        services.AddOptions<RefreshTokenOptions>()
            .Bind(configuration.GetSection("RefreshToken"))
            .Validate(opts => !string.IsNullOrWhiteSpace(opts.Secret) && opts.ExpiresInHours > 0) 
            .ValidateOnStart();

        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddSingleton<IRefreshTokenValidator, RefreshTokenValidator>();

        services
            .ConfigureOptions<AccessTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }
}