using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ReferenceFeature;

public class Cryptography
{
    private static readonly Random _random = new();

    public static string RandomGuid() => Guid.NewGuid().ToString();

    public static string Md5(string str) =>
        BitConverter.ToString(MD5.HashData(Encoding.UTF8.GetBytes(str))).Replace("-", "");

    public static string Base64UrlEncode(string input) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).Replace("+", "-").Replace("/", "_").Replace("=", "");

    public static string RandomCode() => _random.NextInt64(100000, 999999).ToString();

    public static string Hash(string content)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        string hashed = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: content!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            )
        );
        return hashed;
    }
}
