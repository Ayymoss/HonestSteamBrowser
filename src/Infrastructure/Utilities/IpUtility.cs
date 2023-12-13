using System.Net;

namespace BetterSteamBrowser.Infrastructure.Utilities;

public static class IpUtility
{
    public static bool IsInCidrRange(this string ipAddress, string cidrNotation)
    {
        var parts = cidrNotation.Split('/');
        var baseAddressSuccess = IPAddress.TryParse(parts[0], out var baseAddress);
        var prefixLengthSuccess = int.TryParse(parts[1], out var prefixLength);
        if (!baseAddressSuccess || !prefixLengthSuccess || baseAddress is null) return false;

        var addressBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
        var baseAddressBytes = baseAddress.GetAddressBytes();

        if (addressBytes.Length != baseAddressBytes.Length) return false;
        var mask = GetNetworkMask(prefixLength, baseAddressBytes.Length);

        return !baseAddressBytes.Where((t, i) => (addressBytes[i] & mask[i]) != (t & mask[i])).Any();
    }

    private static byte[] GetNetworkMask(int prefixLength, int length)
    {
        var mask = new byte[length];
        for (var i = 0; i < mask.Length; i++)
        {
            var bit = prefixLength - i * 8;
            mask[i] = bit switch
            {
                >= 8 => 255,
                > 0 => (byte)(256 - (1 << (8 - bit))),
                _ => mask[i]
            };
        }

        return mask;
    }
}
