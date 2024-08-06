namespace Tgs.GameInterface.LEGaming.Core.Models;

public record GameRecordResp(
	string GameID,
	string Account,
	int ServerID,
	int KindID,
	int TableID,
	int? ChairID,
	int UserCount,
	decimal CellScore,
	decimal AllBet,
	decimal Profit,
	decimal Revenue,
	DateTimeOffset GameStartTime,
	DateTimeOffset GameEndTime,
	string CardValue,
	int ChannelID,
	string LineCode,
	string AdditionalData);
