namespace Tgs.GameInterface.LEGaming.ViewModels;

public record PlayerBalanceResultViewModel(
    decimal LockBalance,
    decimal FreeBalance,
    decimal Balance);
