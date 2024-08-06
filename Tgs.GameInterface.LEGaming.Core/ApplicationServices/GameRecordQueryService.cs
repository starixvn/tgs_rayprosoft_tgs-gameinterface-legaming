using System.Runtime.CompilerServices;
using Tgs.GameInterface.LEGaming.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

internal class GameRecordQueryService(
	IGameRecordApiClient client) : IQueryService<GameRecordQuery, GameRecordResult>
{
	public async IAsyncEnumerable<GameRecordResult> QueryAsync(
		GameRecordQuery query,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var gameRecords = await client
			.GameRecordAsync(
			apiUrl: query.ApiUrl,
			deskey: query.Deskey,
			md5key: query.MD5key,
			agentId: query.AgentId,
			startTime: query.StartTime,
			endTime: query.EndTime,
			cancellationToken: cancellationToken)
			.ToArrayAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameRecordResp>();

		if (!gameRecords.Any())
		{
			yield break;
		}

		foreach (var gameRecord in gameRecords)
		{
			yield return new GameRecordResult(
				PlayerCode: gameRecord.Account,
				ExternalBetId: gameRecord.GameID,
				ExternalGameId: gameRecord.KindID.ToString(),
				ExternalCreatedOn: gameRecord.GameStartTime,
				ExternalClosedOn: gameRecord.GameEndTime,
				Bet: gameRecord.AllBet,
				ValidBet: gameRecord.CellScore,
				Settle: gameRecord.AllBet + gameRecord.Profit,
				Commission: gameRecord.Revenue,
				WinLoss: gameRecord.Profit,
				AdditionalData: gameRecord.AdditionalData);
		}
	}
}
