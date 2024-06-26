using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest;

public record DepositUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var policySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			DepositRetryCount = 1,
			DepositRetryWaitSec = 1,
			GameRecordRetryCount = 1,
			GameRecordRetryWaitSec = 1
		});

		Uri apiUrl = new Uri("https://wc1-api.kewmn686.com/");
		var deskey = "706EB65DD6E6DF4C";
		var md5key = "3E00054B89B31B7A";
		var agentId = "200995";
		var account = "4uk9399rb9qo";
		var orderId = "200995202307180240242304uk9399rb9qo";
		var money = 1.0m;

		var fakeCryptography = Substitute.For<IHashCryptography>();
		var fakeSystemClock = Substitute.For<ISystemClock>();

		var currentTime = DateTimeOffset.Parse("2023-07-18T02:40:24.230+00:00");
		_ = fakeSystemClock.CurrentTime.Returns(currentTime);

		var data = "s=2&account=4uk9399rb9qo&money=1.0&orderid=200995202307180240242304uk9399rb9qo";
		var param = "9BKbWenbOpD7JD8jm2XOGDdvMatt5jkN9bInYdABoiOAeTiwMbg4NxDsLp7/LhHd1dFKzy51pD2g331dMTPBXEg8ygcwLrvnz9PsPIDwJOM=";
		_ = fakeCryptography.GetAesEncryptAsync(
			data: Arg.Is(data),
			secretKey: Arg.Is(deskey),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(param);

		var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
		var key = "dc31fd6ee81fe24bff9a0ff3d8942e37";
		_ = fakeCryptography.GetHash(
			data: Arg.Is(dataMD5))
			.Returns(key);

		var uriBuilder = new UriBuilder(new Uri(apiUrl, "/channelHandle"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["agent"] = agentId;
		query["timestamp"] = currentTime.ToUnixTimeMilliseconds().ToString();
		query["param"] = param;
		query["key"] = key;
		uriBuilder.Query = query.ToString();

		var response = new JsonObject
		{
			["m"] = "/channelHandle",
			["s"] = 102,
			["d"] = new JsonObject
			{
				["code"] = 0,
				["money"] = "1.00"
			}
		};

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			policySettingsOptions,
			fakeCryptography,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.DepositAsync(
			apiUrl: apiUrl,
			deskey: deskey,
			md5key: md5key,
			agentId: agentId,
			account: account,
			orderId: orderId,
			money: money);

		Assert.NotNull(actual);
		Assert.Equal(response.ToJsonString(), actual.Resp);
	}

	[Fact]
	public async Task Code_Is_39()
	{
		var policySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			DepositRetryCount = 1,
			DepositRetryWaitSec = 1,
			GameRecordRetryCount = 1,
			GameRecordRetryWaitSec = 1
		});

		Uri apiUrl = new Uri("https://wc1-api.kewmn686.com/");
		var deskey = "706EB65DD6E6DF4C";
		var md5key = "3E00054B89B31B7A";
		var agentId = "200995";
		var account = "4uk9399rb9qo";
		var orderId = "200995202307180240242304uk9399rb9qo";
		var money = 1.0m;

		var fakeCryptography = Substitute.For<IHashCryptography>();
		var fakeSystemClock = Substitute.For<ISystemClock>();

		var currentTime = DateTimeOffset.Parse("2023-07-18T02:40:24.230+00:00");
		_ = fakeSystemClock.CurrentTime.Returns(currentTime);

		var data = "s=2&account=4uk9399rb9qo&money=1.0&orderid=200995202307180240242304uk9399rb9qo";
		var param = "9BKbWenbOpD7JD8jm2XOGDdvMatt5jkN9bInYdABoiOAeTiwMbg4NxDsLp7/LhHd1dFKzy51pD2g331dMTPBXEg8ygcwLrvnz9PsPIDwJOM=";
		_ = fakeCryptography.GetAesEncryptAsync(
			data: Arg.Is(data),
			secretKey: Arg.Is(deskey),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(param);

		var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
		var key = "dc31fd6ee81fe24bff9a0ff3d8942e37";
		_ = fakeCryptography.GetHash(
			data: Arg.Is(dataMD5))
			.Returns(key);

		var uriBuilder = new UriBuilder(new Uri(apiUrl, "/channelHandle"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["agent"] = agentId;
		query["timestamp"] = currentTime.ToUnixTimeMilliseconds().ToString();
		query["param"] = param;
		query["key"] = key;
		uriBuilder.Query = query.ToString();

		var response = new JsonObject
		{
			["m"] = "/channelHandle",
			["s"] = 102,
			["d"] = new JsonObject
			{
				["code"] = 39
			}
		};

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			policySettingsOptions,
			fakeCryptography,
			fakeSystemClock,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<MockHttpMatchException>(
			() => client.DepositAsync(
				apiUrl: apiUrl,
				deskey: deskey,
				md5key: md5key,
				agentId: agentId,
				account: account,
				orderId: orderId,
				money: money)
			.AsTask());
	}
}
