using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.IntegrationTest;

public class GameInterfaceLEGamingClient_PlayerLogoutUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://wc1-api.kewmn686.com/");
		var deskey = "706EB65DD6E6DF4C";
		var md5key = "3E00054B89B31B7A";
		var agentId = "200995";
		var playerCode = "4uk9399rb9qo";

		var fakeService = Substitute.For<ICommandService<PlayerLogoutCommand>>();
		var web = new GameInterfaceLEGamingApplication(builder => _ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeService)));

		var sut = new Client.GameInterfaceLEGamingClient(new Protos.GameInterfaceLEGaming.GameInterfaceLEGamingClient(
			GrpcChannel.ForAddress(
				address: web.Server.BaseAddress,
				channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		await sut.PlayerLogoutAsync(apiUrl, deskey, md5key, agentId, playerCode);

		_ = fakeService
			.Received(1)
			.ExecuteAsync(
			command: Arg.Is(new PlayerLogoutCommand
			{
				ApiUrl = apiUrl,
				Deskey = deskey,
				MD5key = md5key,
				AgentId = agentId,
				PlayerCode = playerCode
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
