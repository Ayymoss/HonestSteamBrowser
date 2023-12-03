using BetterSteamBrowser.Domain.ValueObjects;

namespace BetterSteamBrowser.Domain.Interfaces.Services;

public interface IGameServerPlayerService
{
    List<PlayerInfo>? GetServerPlayersAsync(string serverIp, ushort port, CancellationToken cancellationToken);
}
