using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
	[Fact]
	public async Task Normal_ProcessAsync()
	{
		var query = new FundStatusQuery(
			ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
			Deskey: "706EB65DD6E6DF4C",
			MD5key: "3E00054B89B31B7A",
			AgentId: "200995",
			OrderId: "200995202307180240242304uk9399rb9qo");

		var fakeClient = Substitute.For<IGameApiClient>();

		var service = new FundStatusGetService(fakeClient);

		_ = await service.GetAsync(query: query);

		_ = fakeClient
			.Received(1)
			.OrderStatusAsync(
			Arg.Is(query.ApiUrl),
			Arg.Is(query.Deskey),
			Arg.Is(query.MD5key),
			Arg.Is(query.AgentId),
			Arg.Is(query.OrderId),
			Arg.Any<CancellationToken>());
	}
}
