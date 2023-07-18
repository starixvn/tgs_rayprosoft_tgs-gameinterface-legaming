using System.Runtime.CompilerServices;
using Tgs.GameInterface.LEGaming.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

internal class GameRecordQueryService : IQueryService<GameRecordQuery, GameRecordResult>
{
    private readonly IGameRecordApiClient _client;

    public GameRecordQueryService(
        IGameRecordApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordResult> QueryAsync(
        GameRecordQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var gameRecords = await _client
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
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameRecordResult(
                ExternalRoundId: gameRecord.GameID,
                ExternalGameId: gameRecord.KindID.ToString(),
                PlayerCode: gameRecord.Account,
                ExternalCreatedOn: gameRecord.GameStartTime,
                ExternalClosedOn: gameRecord.GameEndTime,
                Bet: gameRecord.AllBet,
                ValidBet: gameRecord.AllBet,
                Settle: gameRecord.AllBet + gameRecord.Profit,
                Commission: gameRecord.Revenue,
                Turnover: gameRecord.CellScore,
                WinLoss: gameRecord.Profit,
                AdditionalData: gameRecord.AdditionalData);
        }
    }
}
