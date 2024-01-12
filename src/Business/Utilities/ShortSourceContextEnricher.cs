using Serilog.Core;
using Serilog.Events;

namespace BetterSteamBrowser.Business.Utilities;

public class ShortSourceContextEnricher : ILogEventEnricher
{
    private const int FixedLength = 32;

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (!logEvent.Properties.TryGetValue("SourceContext", out var sourceContext)) return;
        var shortContext = sourceContext.ToString().Split('.').LastOrDefault()?.Trim('"') ?? string.Empty;
        var paddedContext = shortContext.PadLeft(FixedLength, ' ');

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ShortSourceContext", paddedContext));
    }
}
