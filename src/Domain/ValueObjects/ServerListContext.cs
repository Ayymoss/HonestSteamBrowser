using System.Text.Json.Serialization;

namespace BetterSteamBrowser.Domain.ValueObjects;

public class ServerListContext
{
    [JsonPropertyName("servers")] public List<ServerListItem> Servers { get; set; }
}
