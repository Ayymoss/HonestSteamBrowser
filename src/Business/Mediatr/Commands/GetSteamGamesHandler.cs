using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Domain.Interfaces;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetSteamGamesHandler(ISteamGameRepository steamGameRepository) : IRequestHandler<GetSteamGamesCommand, List<SteamGame>>
{
    public async Task<List<SteamGame>> Handle(GetSteamGamesCommand request, CancellationToken cancellationToken)
    {
        var result = (await steamGameRepository.GetSteamGamesAsync())
            .Where(x => x.Id is not SteamGameConstants.Unknown)
            .Where(x => x.Id is not SteamGameConstants.AllGames)
            .Select(x => new SteamGame
            {
                AppId = x.AppId,
                Name = x.Name
            }).ToList();
        return result;
    }
}
