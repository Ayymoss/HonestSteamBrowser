using System.Security.Principal;
using System.Text.RegularExpressions;

namespace BetterSteamBrowser.WebCore.Utilities;

public static partial class ExtensionMethods
{
    public static bool IsInEqualOrHigherRole<TEnum>(this IPrincipal principal, TEnum role) where TEnum : Enum
    {
        var roleValue = Convert.ToInt32(role);

        var higherRoles = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Where(r => Convert.ToInt32(r) >= roleValue);

        return higherRoles.Any(higherRole => principal.IsInRole(higherRole.ToString()));
    }
}
