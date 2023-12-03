using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class UpdateInformationCommand : INotification;

public class BlacklistServerAddressCommand : INotification
{
    public string IpAddress { get; set; }
    public int SteamGameId { get; set; }
}

public class ToggleFavouriteServerCommand : INotification
{
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public string UserId { get; set; }
}
