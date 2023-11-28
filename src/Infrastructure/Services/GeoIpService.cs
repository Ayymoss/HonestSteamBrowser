using System.Reflection;
using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects;
using MaxMind.GeoIP2;

namespace BetterSteamBrowser.Infrastructure.Services;

public class GeoIpService(SetupConfigurationContext configuration) : IGeoIpService
{
    public void PopulateCountries(IEnumerable<EFServer> servers)
    {
        var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var fileName = Path.Join(workingDirectory, "Resources", configuration.MaxMindGeoIp2DatabaseName);
        var parallelOptions = new ParallelOptions {MaxDegreeOfParallelism = 10};
        using var reader = new DatabaseReader(fileName);

        Parallel.ForEach(servers, parallelOptions, server =>
        {
            try
            {
                // This "Captured variable is disposed in the outer scope" is a lie.
                var response = reader.Country(server.Address.Split(':').First());
                server.Country = response.Country.Name;
                server.CountryCode = response.Country.IsoCode;
            }
            catch
            {
                // ignored
            }
        });
    }
}
