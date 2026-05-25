using System;
using back_yahad.Modules.Auth.DTOs;
using back_yahad.Modules.Auth.Services;

namespace back_yahad.Modules.Auth.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app
    )
    {
        var grupo = app.MapGroup("/auth")
            .WithTags("Auth");

        grupo.MapPost("/login",
            async (
                LoginRequest dto,
                AuthService authService,
                CancellationToken ct
            ) =>
            {
                var token = await authService.LoginAsync(
                    dto.Email,
                    dto.Password,
                    ct
                );

                if (token is null)
                    return Results.Unauthorized();

                return Results.Ok(
                    new LoginResponse(token)
                );
            });
        return app;
    }
}
