namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record PlayerBalanceResult(
    decimal LockBalance,
    decimal FreeBalance,
    decimal Balance);
