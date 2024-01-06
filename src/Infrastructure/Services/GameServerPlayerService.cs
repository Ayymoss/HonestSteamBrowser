using BetterSteamBrowser.Domain.Interfaces.Services;
using QueryMaster;
using QueryMaster.GameServer;
using PlayerInfo = BetterSteamBrowser.Domain.ValueObjects.PlayerInfo;

namespace BetterSteamBrowser.Infrastructure.Services;

public class GameServerPlayerService : IGameServerPlayerService
{
    public List<PlayerInfo>? GetServerPlayersAsync(string serverIp, ushort port, CancellationToken cancellationToken)
    {
        var task = Task.Run(() =>
        {
            // I don't know a better way to deal with this... Some servers' query respond on the game port, some on the next one, some on a completely different one.
            // Is there a better way to reliably get the query port?
            // This approach is problematic as if 'server.GetPlayers()' hangs from the library, the timeout will kill this task before we can try the next port.
            // We can't have the timeout too long either as it hangs the UI thread. This really needs to be a background thread. 
            // It's only a problem with a few servers, but it's still a problem.
            using var server = ServerQuery.GetServerInstance(EngineType.Source, serverIp, port);
            using var query = ServerQuery.GetServerInstance(EngineType.Source, serverIp, ++port);
            var players = server.GetPlayers();
            players ??= query.GetPlayers();

            var playerInfos = players?.Select(x => new PlayerInfo
            {
                Name = x.Name,
                Score = x.Score,
                Online = x.Time
            }).ToList();

            return playerInfos;
        }, cancellationToken);

        try
        {
            return task.Wait(TimeSpan.FromMilliseconds(2_500), cancellationToken) ? task.Result : null;
        }
        catch
        {
            return null;
        }
    }
}
