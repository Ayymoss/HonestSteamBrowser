using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.ValueObjects;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetSteamGamesHandler(ISteamGameRepository steamGameRepository) : IRequestHandler<GetSteamGamesCommand, List<SteamGame>>
{
    public async Task<List<SteamGame>> Handle(GetSteamGamesCommand request, CancellationToken cancellationToken)
    {
        var result = (await steamGameRepository.GetSteamGamesAsync(cancellationToken))
            .Where(x => x.Id is not SteamGameConstants.Unknown)
            .Where(x => x.Id is not SteamGameConstants.AllGames)
            .Select(x => new SteamGame
            {
                Id = x.Id,
                AppId = x.AppId,
                Name = x.Name
            }).ToList();
        return result;
    }
}
