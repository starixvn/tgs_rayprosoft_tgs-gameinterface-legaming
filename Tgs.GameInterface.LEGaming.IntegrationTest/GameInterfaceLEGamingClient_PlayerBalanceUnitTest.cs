using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.IntegrationTest;

public class GameInterfaceLEGamingClient_PlayerBalanceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://wc1-api.kewmn686.com/");
		var deskey = "706EB65DD6E6DF4C";
		var md5key = "3E00054B89B31B7A";
		var agentId = "200995";
		var agentCurrency = "CNY";
		var playerCode = "4uk9399rb9qo";

		var result = new PlayerBalanceResult
		{
			IsPlayerExist = true,
			Balance = 100,
			FreeBalance = 98
		};

		var web = new GameInterfaceLEGamingApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<PlayerBalanceQuery, PlayerBalanceResult>>();

			var query = new PlayerBalanceQuery
			{
				ApiUrl = apiUrl,
				Deskey = deskey,
				MD5key = md5key,
				AgentId = agentId,
				AgentCurrency = agentCurrency,
				PlayerCode = playerCode
			};

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceLEGamingClient(new Protos.GameInterfaceLEGaming.GameInterfaceLEGamingClient(
			GrpcChannel.ForAddress(
				address: web.Server.BaseAddress,
				channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.PlayerBalanceAsync(apiUrl, deskey, md5key, agentId, agentCurrency, playerCode);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(100, actual.Balance);
		Assert.Equal(98, actual.FreeBalance);
		Assert.Equal(2, actual.OutstandingBalance);
	}
}
