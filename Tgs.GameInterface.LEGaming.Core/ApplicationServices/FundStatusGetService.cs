using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public class FundStatusGetService(
    IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
    public ValueTask<bool> GetAsync(
        FundStatusQuery query,
        CancellationToken cancellationToken = default)
        => client
        .OrderStatusAsync(
            apiUrl: query.ApiUrl,
            deskey: query.Deskey,
            md5key: query.MD5key,
            agentId: query.AgentId,
            orderId: query.OrderId,
            cancellationToken: cancellationToken);
}
