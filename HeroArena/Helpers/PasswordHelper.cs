using System.Security.Cryptography;
using System.Text;

namespace HeroArena.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }

    public static bool Verify(string password, string hash)
        => HashPassword(password) == hash;
}