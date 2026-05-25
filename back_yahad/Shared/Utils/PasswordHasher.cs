using BCrypt.Net;

namespace back_yahad.Shared.Utils;

public static class PasswordHasher
{
    public static string Hash(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public static bool Verify(string senha, string senhaHash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, senhaHash);
    }
}