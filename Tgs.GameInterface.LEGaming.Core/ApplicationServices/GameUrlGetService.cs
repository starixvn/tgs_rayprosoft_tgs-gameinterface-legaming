using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public class GameUrlGetService(
	IGameApiClient client) : IGetService<GameUrlQuery, string>
{
	public ValueTask<string> GetAsync(
		GameUrlQuery query,
		CancellationToken cancellationToken = default)
		=> client.LoginAsync(
			apiUrl: query.ApiUrl,
			deskey: query.Deskey,
			md5key: query.MD5key,
			agentId: query.AgentId,
			currency: query.AgentCurrency,
			account: query.PlayerCode,
			ipAddress: query.IpAddress,
			lineCode: query.BrandCode,
			kindID: query.ExternalGameId,
			cancellationToken: cancellationToken);
}
