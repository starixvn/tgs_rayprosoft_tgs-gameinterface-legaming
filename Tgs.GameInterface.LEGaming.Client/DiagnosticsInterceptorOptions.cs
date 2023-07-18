using System.Diagnostics;

namespace Tgs.GameInterface.LEGaming.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.LEGaming.Client.DiagnosticsInterceptor");
}
