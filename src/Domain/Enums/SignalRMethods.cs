namespace BetterSteamBrowser.Domain.Enums;

public enum SignalRMethods
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
