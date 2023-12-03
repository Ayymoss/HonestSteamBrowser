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
            using var server = ServerQuery.GetServerInstance(EngineType.Source, serverIp, port);
            var players = server.GetPlayers();

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
            return task.Wait(TimeSpan.FromSeconds(2), cancellationToken) ? task.Result : null;
        }
        catch
        {
            return null;
        }
    }
}
