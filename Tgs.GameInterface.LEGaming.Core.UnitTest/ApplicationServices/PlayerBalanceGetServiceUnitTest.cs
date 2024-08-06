using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.GameInterface.LEGaming.Core.Models;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_ProcessAsync()
	{
		var query = new PlayerBalanceQuery
		{
			ApiUrl = new Uri("https://wc1-api.kewmn686.com/"),
			Deskey = "706EB65DD6E6DF4C",
			MD5key = "3E00054B89B31B7A",
			AgentId = "200995",
			AgentCurrency = "CNY",
			PlayerCode = "4uk9399rb9qo"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new BalanceResp(
			IsPlayerExist: true,
			TotalMoney: 100,
			FreeMoney: 98);
		_ = fakeClient.BalanceAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			deskey: Arg.Is(query.Deskey),
			md5key: Arg.Is(query.MD5key),
			agentId: Arg.Is(query.AgentId),
			currency: Arg.Is(query.AgentCurrency),
			account: Arg.Is(query.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(resp);

		var service = new PlayerBalanceGetService(fakeClient);

		var actual = await service.GetAsync(query: query);

		Assert.NotNull(actual);
		Assert.True( actual.IsPlayerExist);
		Assert.Equal(100, actual.Balance);
		Assert.Equal(98, actual.FreeBalance);
		Assert.Equal(2, actual.OutstandingBalance);
	}
}
