using Cleanly.Application.Common.Interfaces.Services;
using Cleanly.Application.Common.Security;
using Cleanly.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cleanly.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<ICleanerService, CleanerService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }
}
