using System.Collections.Concurrent;
using System.Net;
using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Services;

public record AutonomousSystemData(long? AutonomousSystemNumber, string? AutonomousSystemOrganization);

public interface IGeoIpService
{
    void PopulateIpContext(IEnumerable<EFServer> servers);
    ConcurrentDictionary<string, AutonomousSystemData> PopulateAsns(IEnumerable<IPAddress> ipAddresses);
    string? GetAutonomousSystemOrganization(string ipAddress);
}
