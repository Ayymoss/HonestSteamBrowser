using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class UpdateInformationCommand : INotification;

public class BlacklistServerAddressCommand : INotification
{
    public string IpAddress { get; set; }
    public int SteamGameId { get; set; }
}
