using System.Net;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.IntegrationTest;
public class GameInterfaceLEGamingClient_GameUrlUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://wc1-api.kewmn686.com/");
		var deskey = "706EB65DD6E6DF4C";
		var md5key = "3E00054B89B31B7A";
		var agentId = "200995";
		var agentCurrency = "CNY";
		var brandCode = "GPI";
		var playerCode = "4uk9399rb9qo";
		var ipAddress = "127.0.0.1";
		var externalGameId = "0";

		var result = new PlayerBalanceResult
		{
			Balance = 100,
			FreeBalance = 98
		};

		var web = new GameInterfaceLEGamingApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<GameUrlQuery, string>>();

			var query = new GameUrlQuery
			{
				ApiUrl = apiUrl,
				Deskey = deskey,
				MD5key = md5key,
				AgentId = agentId,
				AgentCurrency = agentCurrency,
				BrandCode = brandCode,
				PlayerCode = playerCode,
				IpAddress = ipAddress,
				ExternalGameId = externalGameId
			};

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns("https://wc1-g-h5.kewmn686.com/");

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceLEGamingClient(new Protos.GameInterfaceLEGaming.GameInterfaceLEGamingClient(
			GrpcChannel.ForAddress(
				address: web.Server.BaseAddress,
				channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.GameUrlAsync(apiUrl, deskey, md5key, agentId, agentCurrency, brandCode, playerCode, ipAddress, externalGameId);

		Assert.NotNull(actual);
		Assert.NotEmpty(actual);
	}
}
