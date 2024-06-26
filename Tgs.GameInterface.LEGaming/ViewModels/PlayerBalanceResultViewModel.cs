namespace Tgs.GameInterface.LEGaming.ViewModels;

public record PlayerBalanceResultViewModel(
	decimal Balance,
	decimal FreeBalance,
	decimal OutstandingBalance);
