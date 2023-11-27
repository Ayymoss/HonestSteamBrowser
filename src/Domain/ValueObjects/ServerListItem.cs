using System.Text.Json.Serialization;
using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Domain.ValueObjects;

public class ServerListItem
{
    [JsonPropertyName("addr")] public string Address { get; set; }
    [JsonPropertyName("gameport")] public int GamePort { get; set; }
    [JsonPropertyName("steamid")] public string SteamId { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("appid")] public int AppId { get; set; }
    [JsonPropertyName("gamedir")] public string GameDirectory { get; set; }
    [JsonPropertyName("version")] public string Version { get; set; }
    [JsonPropertyName("product")] public string Product { get; set; }
    [JsonPropertyName("region")] public int Region { get; set; }
    [JsonPropertyName("players")] public int Players { get; set; }
    [JsonPropertyName("max_players")] public int MaxPlayers { get; set; }
    [JsonPropertyName("bots")] public int Bots { get; set; }
    [JsonPropertyName("map")] public string? Map { get; set; }
    [JsonPropertyName("secure")] public bool Secure { get; set; }
    [JsonPropertyName("dedicated")] public bool Dedicated { get; set; }
    [JsonPropertyName("os")] public string OperatingSystem { get; set; }
    [JsonPropertyName("gametype")] public string GameType { get; set; }
}
