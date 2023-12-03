using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetServerPlayersHandler(IGameServerPlayerService gameServerPlayerService) : IRequestHandler<GetServerPlayersCommand, List<PlayerInfo>?>
{
    public Task<List<PlayerInfo>?> Handle(GetServerPlayersCommand request, CancellationToken cancellationToken)
    {
        var players = gameServerPlayerService.GetServerPlayersAsync(request.IpAddress, (ushort)request.Port, cancellationToken);
        return Task.FromResult(players);
    }
}
