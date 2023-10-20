using Tgs.GameInterface.LEGaming.Core.Models;

namespace Tgs.GameInterface.LEGaming.Core;

public interface IGameRecordApiClient
{
    IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string deskey, string md5key, string agentId, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);
}
