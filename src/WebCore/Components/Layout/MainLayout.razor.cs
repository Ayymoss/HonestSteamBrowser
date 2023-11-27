using Radzen.Blazor;

namespace BetterSteamBrowser.WebCore.Components.Layout;

public partial class MainLayout : IDisposable
{
    private RadzenBody? _body;

    public void Dispose()
    {
        _body?.Dispose();
    }
}
