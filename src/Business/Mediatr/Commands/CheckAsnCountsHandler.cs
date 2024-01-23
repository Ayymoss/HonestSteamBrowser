using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.ValueObjects;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class CheckAsnCountsHandler(IServerRepository serverRepository) : IRequestHandler<CheckAsnCountsCommand, List<AsnPreBlock>>
{
    public async Task<List<AsnPreBlock>> Handle(CheckAsnCountsCommand request, CancellationToken cancellationToken)
    {
        var result = await serverRepository.GetAsnBlockListAsync(request.AutonomousSystemOrganization, request.SteamGameId, cancellationToken);
        return result;
    }
}
