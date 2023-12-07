using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetBlockCountHandler(IBlockRepository blockRepository) : IRequestHandler<GetBlockCountCommand, int>
{
    public async Task<int> Handle(GetBlockCountCommand request, CancellationToken cancellationToken)
    {
        var count = await blockRepository.CountAsync(cancellationToken);
        return count;
    }
}
