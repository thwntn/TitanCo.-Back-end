namespace ReferenceFeature;

public class Cryptography
{
    private static readonly Random _random = new();

    public static string RandomGuid() => Guid.NewGuid().ToString();

    public static string Md5(string str) =>
        BitConverter.ToString(MD5.HashData(Encoding.UTF8.GetBytes(str))).Replace("-", "");

    public static string Base64UrlEncode(string input) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).Replace("+", "-").Replace("/", "_").Replace("=", "");

    public static long RandomCode() => _random.NextInt64(100000, 999999);
}
