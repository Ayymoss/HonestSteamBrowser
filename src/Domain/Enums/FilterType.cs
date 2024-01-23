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
    CountryCode,
    [Display(Description = "CIDR Subnet")]
    Subnet,
    [Display(Description = "Autonomous System Organization")]
    AutonomousSystemOrganization
}
