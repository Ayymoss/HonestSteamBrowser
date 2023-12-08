using System.ComponentModel.DataAnnotations;

namespace BetterSteamBrowser.Domain.Enums;

public enum FilterType
{
    [Display(Description = "IP Address")]
    IpAddress,
    [Display(Description = "Game Type")]
    GameType,
    [Display(Description = "Hostname")]
    Hostname,
    [Display(Description = "Country Code")]
    CountryCode
}
