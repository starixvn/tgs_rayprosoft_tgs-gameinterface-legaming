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

public class BalanceUnitTest
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

		var fakeCryptography = Substitute.For<IHashCryptography>();
		var fakeSystemClock = Substitute.For<ISystemClock>();

		var currentTime = DateTimeOffset.FromUnixTimeMilliseconds(1689585314239);
		_ = fakeSystemClock.CurrentTime.Returns(currentTime);

		var data = "s=7&account=4uk9399rb9qo";
		var param = "LxLJVtx15H2dgXaCSJNG9OQRmoSZ5fdqQNxK502gobo=";
		_ = fakeCryptography.GetAesEncryptAsync(
			data: Arg.Is(data),
			secretKey: Arg.Is(deskey),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(param);

		var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
		var key = "cc9b6211f41b5234b25ebd69be179a46";
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
			["s"] = 107,
			["d"] = new JsonObject
			{
				["code"] = 0,
				["account"] = "200995_4uk9399rb9qo",
				["totalMoney"] = 0,
				["freeMoney"] = 0,
				["status"] = 0,
				["gameStatus"] = 0
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

		var actual = await client.BalanceAsync(
			apiUrl: apiUrl,
			deskey: deskey,
			md5key: md5key,
			agentId: agentId,
			account: account);

		Assert.NotNull(actual);
		Assert.Equal(0.00m, actual.FreeMoney);
	}
}
