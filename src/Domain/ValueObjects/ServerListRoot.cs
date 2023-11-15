using System.Text.Json.Serialization;

namespace BetterSteamBrowser.Domain.ValueObjects;

public class ServerListRoot
{
    [JsonPropertyName("response")] public ServerListContext Response { get; set; }
}
