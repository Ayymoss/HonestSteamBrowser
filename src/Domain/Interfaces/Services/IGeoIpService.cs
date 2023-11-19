using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Services;

public interface IGeoIpService
{
    IEnumerable<EFServer> PopulateCountries(IEnumerable<EFServer> servers);
}
