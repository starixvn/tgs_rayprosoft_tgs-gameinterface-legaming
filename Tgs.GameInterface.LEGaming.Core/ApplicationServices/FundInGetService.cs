using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

internal class FundInGetService(
	IGameApiClient client) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await client.WithdrawAsync(
			apiUrl: query.ApiUrl,
			deskey: query.Deskey,
			md5key: query.MD5key,
			agentId: query.AgentId,
			currency: query.AgentCurrency,
			account: query.PlayerCode,
			orderId: query.OrderId,
			money: query.Amount,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new FundResult(resp.Resp);
	}
}
