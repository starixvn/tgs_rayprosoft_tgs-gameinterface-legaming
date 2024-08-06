namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record GameRecordResult(
	string PlayerCode,
	string ExternalBetId,
	string ExternalGameId,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal Commission,
	decimal WinLoss,
	string AdditionalData);
