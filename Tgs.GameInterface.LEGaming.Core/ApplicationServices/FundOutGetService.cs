using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

internal class FundOutGetService(
	IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await client.DepositAsync(
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
