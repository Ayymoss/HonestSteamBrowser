using BetterSteamBrowser.Domain.Enums;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class UpdateInformationCommand : INotification;

public class BlockServerAddressCommand : INotification
{
    public string IpAddress { get; set; }
    public int SteamGameId { get; set; }
    public string UserId { get; set; }
}

public class ToggleFavouriteServerCommand : INotification
{
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public string UserId { get; set; }
}

public class CreateBlockCommand : INotification
{
    public string Value { get; set; }
    public bool ApiFilter { get; set; }
    public FilterType Type { get; set; }
    public int SteamGameId { get; set; }
    public string UserId { get; set; }
}

public class RemoveBlockCommand : INotification
{
    public int Id { get; set; }
}

public class RemoveSteamGameCommand : INotification
{
    public int Id { get; set; }
}

public class CreateSteamGameCommand : INotification
{
    public int AppId { get; set; }
    public string Name { get; set; }
}

public class ChangeUserRoleCommand : INotification
{
    public string Id { get; set; }
    public bool IsAdmin { get; set; }
}

public class ResetUserPasswordCommand : INotification
{
    public string Id { get; set; }
    public string? Password { get; set; }
}

public class BlockAsnCommand : INotification
{
    public string AutonomousSystemOrganization { get; set; }
    public string UserId { get; set; }
    public int SteamGameId { get; set; }
}
