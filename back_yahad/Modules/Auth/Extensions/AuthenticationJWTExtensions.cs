using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace back_yahad.Modules.Auth.Extensions;

public static class AuthenticationJWTExtensions
{
    public static IServiceCollection AddAuthenticationModule(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var key = config["Jwt:Key"]!;
        var issuer = config["Jwt:Issuer"]!;
        var audience = config["Jwt:Audience"]!;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = issuer,
                        ValidAudience = audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(key)
                            )
                    };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "AdminPolicy",
                policy => policy.RequireRole("admin")
            );

            options.AddPolicy(
                "UserPolicy",
                policy => policy.RequireRole("usuario")
            );
        });

        return services;
    }
}