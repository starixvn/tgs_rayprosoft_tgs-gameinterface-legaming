using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.IntegrationTest;

public class GameInterfaceLEGamingClient_FundInUnitTest
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
		var orderId = "200995202307180247377734uk9399rb9qo";
		var amount = 1.0m;

		var result = new FundResult(Resp: "{\"m\":\"/channelHandle\",\"s\":103,\"d\":{\"code\":0,\"money\":\"0.00\"}}");

		var web = new GameInterfaceLEGamingApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<FundInQuery, FundResult>>();

			var query = new FundInQuery
			{
				ApiUrl = apiUrl,
				Deskey = deskey,
				MD5key = md5key,
				AgentId = agentId,
				AgentCurrency = agentCurrency,
				PlayerCode = playerCode,
				OrderId = orderId,
				Amount = amount
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

		var actual = await sut.FundInAsync(apiUrl, deskey, md5key, agentId, agentCurrency, playerCode, orderId, amount);

		Assert.NotNull(actual);
		Assert.NotEmpty(actual.Resp);
	}
}
