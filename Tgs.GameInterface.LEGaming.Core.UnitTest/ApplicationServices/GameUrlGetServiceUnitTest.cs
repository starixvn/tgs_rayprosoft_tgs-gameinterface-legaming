using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
	[Fact]
	public async Task Normal_ProcessAsync()
	{
		var query = new GameUrlQuery
		{
			ApiUrl = new Uri("https://wc1-api.kewmn686.com/"),
			Deskey = "706EB65DD6E6DF4C",
			MD5key = "3E00054B89B31B7A",
			AgentId = "200995",
			AgentCurrency = "CNY",
			BrandCode = "GPI",
			PlayerCode = "4uk9399rb9qo",
			IpAddress = "127.0.0.1",
			ExternalGameId = "0"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var service = new GameUrlGetService(fakeClient);

		_ = await service.GetAsync(query: query);

		_ = fakeClient
			.Received(1)
			.LoginAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			deskey: Arg.Is(query.Deskey),
			md5key: Arg.Is(query.MD5key),
			agentId: Arg.Is(query.AgentId),
			currency: Arg.Is(query.AgentCurrency),
			account: Arg.Is(query.PlayerCode),
			ipAddress: Arg.Is(query.IpAddress),
			lineCode: Arg.Is(query.BrandCode),
			kindID: Arg.Is(query.ExternalGameId),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
