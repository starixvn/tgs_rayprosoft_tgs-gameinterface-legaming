using System.Diagnostics;

namespace Tgs.GameInterface.LEGaming;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.LEGaming.DiagnosticsInterceptor");
}
