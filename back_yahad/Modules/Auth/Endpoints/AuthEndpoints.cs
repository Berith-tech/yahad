using back_yahad.Modules.Auth.Domain;
using back_yahad.Modules.Auth.DTOs;
using back_yahad.Modules.Auth.Repositories;
using back_yahad.Modules.Auth.Services;
using back_yahad.Modules.Users.Repositories;
using back_yahad.Shared.Utils;

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
                var token = await authService.LoginAsync(dto.Email, dto.Password, ct);

                if (token is null)
                    return Results.Unauthorized();

                return Results.Ok(new LoginResponse(token));
            });

        grupo.MapPost("/forgot-password",
            async (
                ForgotPasswordRequest dto,
                IUsuarioRepository usuarioRepo,
                IPasswordResetTokenRepository tokenRepo,
                IEmailService emailService,
                IConfiguration config,
                CancellationToken ct
            ) =>
            {
                var genericResponse = Results.Ok(new
                {
                    message = "If this email is registered, you will receive instructions shortly."
                });

                var usuario = await usuarioRepo.GetByEmailAsync(dto.Email, ct);
                if (usuario is null)
                    return genericResponse;

                var rawToken = System.Security.Cryptography.RandomNumberGenerator
                    .GetHexString(64);

                var expirationMinutes = config.GetValue<int>("PasswordReset:ExpirationMinutes", 60);
                var resetToken = new PasswordResetToken
                {
                    UserId = usuario.Id,
                    Token = rawToken,
                    ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(expirationMinutes),
                    CreatedAt = DateTimeOffset.UtcNow
                };

                await tokenRepo.AddAsync(resetToken, ct);

                var baseUrl = config["PasswordReset:BaseUrl"] ?? "http://localhost:4200";
                var resetLink = $"{baseUrl}/reset-password?token={rawToken}";

                await emailService.SendPasswordResetAsync(dto.Email, resetLink, ct);

                return genericResponse;
            });

        grupo.MapPost("/reset-password",
            async (
                ResetPasswordRequest dto,
                IPasswordResetTokenRepository tokenRepo,
                IUsuarioRepository usuarioRepo,
                CancellationToken ct
            ) =>
            {
                var resetToken = await tokenRepo.GetValidTokenAsync(dto.Token, ct);
                if (resetToken is null)
                    return Results.BadRequest(new { message = "Invalid or expired token." });

                await usuarioRepo.UpdatePasswordAsync(resetToken.UserId, PasswordHasher.Hash(dto.NewPassword), ct);
                await tokenRepo.MarkAsUsedAsync(resetToken, ct);

                return Results.Ok(new { message = "Password reset successfully." });
            });

        return app;
    }
}
