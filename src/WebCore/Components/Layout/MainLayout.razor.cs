using Radzen.Blazor;

namespace BetterSteamBrowser.WebCore.Components.Layout;

public partial class MainLayout : IAsyncDisposable
{
    private RadzenBody? _body;
    private TopBar? _topBar;

    public async ValueTask DisposeAsync()
    {
        await (_topBar?.DisposeAsync() ?? ValueTask.CompletedTask);
        _body?.Dispose();
    }
}
