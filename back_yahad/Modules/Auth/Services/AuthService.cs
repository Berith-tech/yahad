using back_yahad.Modules.Users.Repositories;
using back_yahad.Shared.Utils;

namespace back_yahad.Modules.Auth.Services;

public class AuthService
{
    private readonly IUsuarioRepository _usuarios;
    private readonly JwtTokenService _jwt;

    public AuthService(
        IUsuarioRepository usuarios,
        JwtTokenService jwt
    )
    {
        _usuarios = usuarios;
        _jwt = jwt;
    }

    public async Task<string?> LoginAsync(
        string email,
        string senha,
        CancellationToken ct = default
    )
    {
        var usuario = await _usuarios.GetByEmailAsync(email, ct);

        if (usuario is null)
            return null;

        var senhaValida = PasswordHasher.Verify(
            senha,
            usuario.SenhaHash
        );

        if (!senhaValida)
            return null;

        return _jwt.GenerateToken(
            usuario.Id.ToString(),
            usuario.Email,
            usuario.Role?.Nome ?? "User"
        );
    }
}