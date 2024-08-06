using System.Text.Json.Nodes;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.IntegrationTest;

public class GameInterfaceLEGamingClient_GameRecordUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://wc1-api.kewmn686.com/");
		var deskey = "706EB65DD6E6DF4C";
		var md5key = "3E00054B89B31B7A";
		var agentId = "200995";
		var startTime = DateTimeOffset.Parse("2023-07-18T11:59:00.000+08:00");
		var endTime = DateTimeOffset.Parse("2023-07-18T12:09:00.000+08:00");

		var result = new GameRecordResult(
			PlayerCode: "4uk9399rb9qo",
			ExternalBetId: "686-1689653000-19588758-1",
			ExternalGameId: "910",
			ExternalCreatedOn: DateTimeOffset.Parse("2023-07-18T12:03:20+08:00"),
			ExternalClosedOn: DateTimeOffset.Parse("2023-07-18T12:03:44+08:00"),
			Bet: 6.0m,
			ValidBet: 2.0m,
			Settle: 4.0m,
			Commission: 0.0m,
			WinLoss: -2.0m,
			AdditionalData: new JsonObject
			{
				["GameID"] = "686-1689653000-19588758-1",
				["Accounts"] = "200995_4uk9399rb9qo",
				["ServerID"] = 9101,
				["KindID"] = 910,
				["TableID"] = 182020001,
				["ChairID"] = 0,
				["UserCount"] = 1,
				["CellScore"] = 2.00m,
				["AllBet"] = 6.00m,
				["Profit"] = -2.00m,
				["Revenue"] = 0,
				["GameStartTime"] = "2023-07-18 12:03:20",
				["GameEndTime"] = "2023-07-18 12:03:44",
				["CardValue"] = "0605372c3d011",
				["ChannelID"] = 200995,
				["LineCode"] = "200995_GPI_HKD"
			}.ToJsonString());

		var web = new GameInterfaceLEGamingApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IQueryService<GameRecordQuery, GameRecordResult>>();

			var query = new GameRecordQuery
			{
				ApiUrl = apiUrl,
				Deskey = deskey,
				MD5key = md5key,
				AgentId = agentId,
				StartTime = startTime,
				EndTime = endTime
			};

			_ = fakeService.QueryAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { result }.ToAsyncEnumerable());

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceLEGamingClient(new Protos.GameInterfaceLEGaming.GameInterfaceLEGamingClient(
			GrpcChannel.ForAddress(
				address: web.Server.BaseAddress,
				channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.GameRecordAsync(apiUrl, deskey, md5key, agentId, startTime, endTime).ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal("4uk9399rb9qo", actual.First().PlayerCode);
		Assert.Equal("686-1689653000-19588758-1", actual.First().ExternalBetId);
		Assert.Equal("910", actual.First().ExternalGameId);
		Assert.Equal(DateTimeOffset.Parse("2023-07-18T12:03:20+08:00"), actual.First().ExternalCreatedOn.ToDateTimeOffset());
		Assert.Equal(DateTimeOffset.Parse("2023-07-18T12:03:44+08:00"), actual.First().ExternalClosedOn.ToDateTimeOffset());
		Assert.Equal(6.00m, actual.First().Bet.ToDecimal());
		Assert.Equal(2.00m, actual.First().ValidBet.ToDecimal());
		Assert.Equal(4.00m, actual.First().Settle.ToDecimal());
		Assert.Equal(0.00m, actual.First().Commission.ToDecimal());
		Assert.Equal(-2.00m, actual.First().WinLoss.ToDecimal());
		Assert.NotEmpty(actual.First().AdditionalData);
	}
}
