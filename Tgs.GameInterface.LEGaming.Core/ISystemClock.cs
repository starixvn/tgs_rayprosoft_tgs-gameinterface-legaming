namespace Tgs.GameInterface.LEGaming.Core;

public interface ISystemClock
{
    DateTimeOffset CurrentTime { get; }
}
