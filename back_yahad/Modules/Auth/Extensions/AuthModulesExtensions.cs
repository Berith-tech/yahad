using back_yahad.Modules.Auth.Repositories;
using back_yahad.Modules.Auth.Services;
using Resend;

namespace back_yahad.Modules.Auth.Extensions;

public static class AuthModuleExtensions
{
    public static IServiceCollection AddAuthModule(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddScoped<AuthService>();
        services.AddScoped<JwtTokenService>();
        services.AddScoped<IPasswordResetTokenRepository, EfPasswordResetTokenRepository>();
        services.AddScoped<IEmailService, ResendEmailService>();

        services.AddOptions();
        services.AddHttpClient<ResendClient>();
        services.Configure<ResendClientOptions>(o =>
        {
            o.ApiToken = config["Resend:ApiKey"]
                ?? throw new InvalidOperationException("Resend:ApiKey não configurada.");
        });
        services.AddTransient<IResend, ResendClient>();

        return services;
    }
}