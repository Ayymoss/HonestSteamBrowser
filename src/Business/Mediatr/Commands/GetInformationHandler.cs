using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Business.Services;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetInformationHandler
    (ServerContextCache contextCache, IPublisher publisher) : IRequestHandler<GetInformationCommand, CacheInfo>
{
    public async Task<CacheInfo> Handle(GetInformationCommand request, CancellationToken cancellationToken)
    {
        if (!contextCache.Loaded)
        {
            await publisher.Publish(new UpdateInformationCommand(), cancellationToken);
        }

        var cacheInfo = new CacheInfo
        {
            ServerCount = contextCache.ServerCount,
            PlayerCount = contextCache.PlayerCount
        };

        return cacheInfo;
    }
}
