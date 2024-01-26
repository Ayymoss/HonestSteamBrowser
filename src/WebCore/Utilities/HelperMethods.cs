using System.Reflection;
using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.WebCore.Utilities;

public class HelperMethods
{
    public static string GetHigherRolesAsString(IdentityRole identityRole)
    {
        var identityRoles = Enum.GetValues(typeof(IdentityRole))
            .Cast<IdentityRole>()
            .Where(r => Convert.ToInt32(r) >= (int)identityRole)
            .Select(r => r.ToString())
            .ToList();
        return string.Join(", ", identityRoles);
    }

    public static string GetVersion() => Assembly.GetCallingAssembly().GetName().Version?.ToString() ?? "Unknown";
}
