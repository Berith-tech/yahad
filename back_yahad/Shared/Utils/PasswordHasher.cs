using System.Security.Cryptography;
using System.Text;

namespace back_yahad.Shared.Utils;

public static class PasswordHasher
{
    public static string Hash(string senha)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        return Convert.ToHexString(bytes);
    }
}
