using BetterSteamBrowser.Domain.Interfaces.Services;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetAsnNameHandler(IGeoIpService geoIpService) : IRequestHandler<GetAsnDataCommand, AutonomousSystemData>
{
    public Task<AutonomousSystemData> Handle(GetAsnDataCommand request, CancellationToken cancellationToken)
    {
        var asn = geoIpService.PopulateAsns([request.IpAddress]);
        var result = asn.GetValueOrDefault(request.IpAddress.ToString());
        return Task.FromResult(result);
    }
}
