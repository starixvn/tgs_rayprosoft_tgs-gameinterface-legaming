using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class AgentBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_ProcessAsync()
	{
		var query = new AgentBalanceQuery
		{
			ApiUrl = new Uri("https://wc1-api.kewmn686.com/"),
			Deskey = "706EB65DD6E6DF4C",
			MD5key = "3E00054B89B31B7A",
			AgentId = "200995"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var credit = 10000000.00m;
		_ = fakeClient
			.AgentBalanceAsync(
			Arg.Is(query.ApiUrl),
			Arg.Is(query.Deskey),
			Arg.Is(query.MD5key),
			Arg.Is(query.AgentId),
			Arg.Any<CancellationToken>())
			.Returns(credit);

		var service = new AgentBalanceGetService(
			fakeClient);

		var actual = await service.GetAsync(
			query: query);

		Assert.NotNull(actual);
		Assert.Equal(10000000.00m, actual.Credit);
	}
}
