using System;
using System.Security.Cryptography;
using System.Text;

namespace StringifyMaintenance.Services;

public static class PasswordHasher
{
    public static string CreateSha256(string input)
    {
        using var sha256 = SHA256.Create();
        byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
        foreach (byte value in data)
        {
            builder.Append(value.ToString("x2"));
        }
        return builder.ToString();
    }

    public static string GenerateSalt()
    {
        byte[] saltBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    public static bool VerifyPassword(string password, string salt, string storedHash)
    {
        string tmpHash = CreateSha256(password + salt);
        string finalHash = CreateSha256(tmpHash);
        return string.Equals(finalHash, storedHash, StringComparison.OrdinalIgnoreCase);
    }

    public static string HashWithSalt(string password, string salt)
    {
        string tmpHash = CreateSha256(password + salt);
        return CreateSha256(tmpHash);
    }
}
