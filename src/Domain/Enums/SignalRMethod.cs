namespace BetterSteamBrowser.Domain.Enums;

public enum SignalRMethod
{
    #region Client Methods

    OnInformationUpdated,
    OnActiveUsersUpdate,

    #endregion

    #region Server Methods

    GetInformation,
    GetActiveUsersCount,

    #endregion
}
