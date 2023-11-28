namespace BetterSteamBrowser.Business.DTOs;

public class SteamGame
{
    public int AppId { get; set; }
    public string Name { get; set; }
    
    /// <summary>
    /// Required for RadzenDropDown
    /// </summary>
    /// <returns>string</returns>
    public override string ToString() => Name;
}
