using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Services;

public interface IGeoIpService
{
    void PopulateCountries(IEnumerable<EFServer> servers);
}
