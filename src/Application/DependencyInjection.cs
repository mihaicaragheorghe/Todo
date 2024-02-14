using Application.Common.Behaviors;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            options.AddOpenBehavior(typeof(LoggingBehavior<,>));
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}