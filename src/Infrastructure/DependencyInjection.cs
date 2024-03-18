using Application.Common.Interfaces;

using Domain.TodoLists;
using Domain.Users;

using Infrastructure.Common;
using Infrastructure.Security.TokenGenerator;
using Infrastructure.Security.TokenValidation;
using Infrastructure.TodoLists;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddPersistence(configuration)
            .AddIdentity(configuration);

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoListRepository, TodoListRepository>();

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
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

        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddSingleton<IRefreshTokenValidator, RefreshTokenValidator>();

        services
            .ConfigureOptions<AccessTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }
}