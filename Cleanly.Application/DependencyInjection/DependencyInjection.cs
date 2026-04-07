using Microsoft.Extensions.DependencyInjection;

namespace Cleanly.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services (CQRS handlers, validators, etc.) here.
        return services;
    }
}
