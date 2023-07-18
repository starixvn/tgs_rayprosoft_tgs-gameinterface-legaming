using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public class GameUrlGetService : IGetService<GameUrlQuery, string>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string> GetAsync(
        GameUrlQuery query,
        CancellationToken cancellationToken = default)
        => _client
        .LoginAsync(
            apiUrl: query.ApiUrl,
            deskey: query.Deskey,
            md5key: query.MD5key,
            agentId: query.AgentId,
            account: query.PlayerCode,
            ipAddress: query.IpAddress,
            lineCode: query.BrandCode,
            gameExternalID: query.GameExternalID,
            cancellationToken: cancellationToken);
}
