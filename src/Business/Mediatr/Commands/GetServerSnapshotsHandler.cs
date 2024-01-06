using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.ValueObjects;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetServerSnapshotsHandler(IServerRepository serverRepository)
    : IRequestHandler<GetServerSnapshotsCommand, List<ServerSnapshot>>
{
    public async Task<List<ServerSnapshot>> Handle(GetServerSnapshotsCommand request, CancellationToken cancellationToken)
    {
        var snapshots = await serverRepository.GetServerSnapshotsAsync(request.Hash, cancellationToken);
        var converted = snapshots.Select(x => new ServerSnapshot
        {
            Count = x.SnapshotCount,
            Snapshot = x.SnapshotTaken
        }).ToList();
        return converted;
    }
}
