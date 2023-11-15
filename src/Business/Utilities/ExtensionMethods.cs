using System.Security.Cryptography;
using System.Text;

namespace BetterSteamBrowser.Business.Utilities;

public static class ExtensionMethods
{
    public static string Compute256Hash(this string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}
