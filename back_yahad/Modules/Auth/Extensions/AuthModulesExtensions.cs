using System;
using back_yahad.Modules.Auth.Services;

namespace back_yahad.Modules.Auth.Extensions;

public static class AuthModuleExtensions
{
    public static IServiceCollection AddAuthModule(
        this IServiceCollection services
    )
    {
        services.AddScoped<AuthService>();
        services.AddScoped<JwtTokenService>();

        return services;
    }
}