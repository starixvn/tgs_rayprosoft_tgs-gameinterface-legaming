namespace Tgs.GameInterface.LEGaming.Core;

internal class SystemClock : ISystemClock
{
	public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
}
