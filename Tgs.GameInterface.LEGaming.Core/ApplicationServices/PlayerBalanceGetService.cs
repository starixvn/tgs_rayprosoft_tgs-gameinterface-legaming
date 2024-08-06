using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public class PlayerBalanceGetService(
	IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(
		PlayerBalanceQuery query,
		CancellationToken cancellationToken = default)
	{
		var resp = await client.BalanceAsync(
			apiUrl: query.ApiUrl,
			deskey: query.Deskey,
			md5key: query.MD5key,
			agentId: query.AgentId,
			currency: query.AgentCurrency,
			account: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceResult
		{
			Balance = resp.TotalMoney,
			FreeBalance = resp.FreeMoney
		};
	}
}
