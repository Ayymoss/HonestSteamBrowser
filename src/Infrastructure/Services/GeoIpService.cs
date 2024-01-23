using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects;
using MaxMind.GeoIP2;

namespace BetterSteamBrowser.Infrastructure.Services;

public class GeoIpService(SetupConfigurationContext configuration) : IGeoIpService
{
    public void PopulateIpContext(IEnumerable<EFServer> servers)
    {
        var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var fileNameIpDb = Path.Join(workingDirectory, "_Resources", configuration.MaxMindGeoIp2DatabaseName);
        var fileNameAsnDb = Path.Join(workingDirectory, "_Resources", configuration.MaxMindGeoIp2AsnDatabaseName);
        var parallelOptions = new ParallelOptions {MaxDegreeOfParallelism = 10};
        using var ipReader = new DatabaseReader(fileNameIpDb);
        using var asnReader = new DatabaseReader(fileNameAsnDb);

        Parallel.ForEach(servers, parallelOptions, server =>
        {
            try
            {
                // This "Captured variable is disposed in the outer scope" is a lie.
                // ReSharper disable once AccessToDisposedClosure
                var ipResponse = ipReader.Country(server.IpAddress);
                server.Country = ipResponse.Country.Name;
                server.CountryCode = ipResponse.Country.IsoCode;

                // ReSharper disable once AccessToDisposedClosure
                var asnResponse = asnReader.Asn(server.IpAddress);
                server.AutonomousSystemOrganization = asnResponse.AutonomousSystemOrganization;
            }
            catch
            {
                // ignored
            }
        });
    }

    public ConcurrentDictionary<string, AutonomousSystemData> PopulateAsns(IEnumerable<IPAddress> ipAddresses)
    {
        var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var fileName = Path.Join(workingDirectory, "_Resources", configuration.MaxMindGeoIp2AsnDatabaseName);
        var parallelOptions = new ParallelOptions {MaxDegreeOfParallelism = 10};
        using var reader = new DatabaseReader(fileName);

        var asnData = new ConcurrentDictionary<string, AutonomousSystemData>();
        Parallel.ForEach(ipAddresses, parallelOptions, ipAddress =>
        {
            try
            {
                if (Equals(ipAddress, IPAddress.None)) return;

                // This "Captured variable is disposed in the outer scope" is a lie.
                // ReSharper disable once AccessToDisposedClosure
                var response = reader.Asn(ipAddress);
                if (response.AutonomousSystemNumber.HasValue)
                    asnData.TryAdd(ipAddress.ToString(),
                        new AutonomousSystemData(response.AutonomousSystemNumber.Value, response.AutonomousSystemOrganization));
            }
            catch
            {
                // ignored
            }
        });

        return asnData;
    }

    public string? GetAutonomousSystemOrganization(string ipAddress)
    {
        PopulateAsns([IPAddress.Parse(ipAddress)]).TryGetValue(ipAddress, out var asnData);
        return asnData?.AutonomousSystemOrganization;
    }
}
