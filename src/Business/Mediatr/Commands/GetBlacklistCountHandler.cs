using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetBlacklistCountHandler(IBlacklistRepository blacklistRepository) : IRequestHandler<GetBlacklistCountCommand, int>
{
    public async Task<int> Handle(GetBlacklistCountCommand request, CancellationToken cancellationToken)
    {
        var count = await blacklistRepository.CountAsync(cancellationToken);
        return count;
    }
}
