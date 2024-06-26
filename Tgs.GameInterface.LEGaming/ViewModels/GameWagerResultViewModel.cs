namespace Tgs.GameInterface.LEGaming.ViewModels;

public record GameWagerResultViewModel(
	string ExternalWagerId,
	string ExternalGameId,
	string PlayerCode,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal Commission,
	decimal WinLoss,
	string AdditionalData);
