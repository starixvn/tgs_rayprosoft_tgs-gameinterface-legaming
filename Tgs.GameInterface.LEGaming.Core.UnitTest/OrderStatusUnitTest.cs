using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest;

public class OrderStatusUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        Uri apiUrl = new Uri("https://wc1-api.kewmn686.com/");
        var deskey = "706EB65DD6E6DF4C";
        var md5key = "3E00054B89B31B7A";
        var agentId = "200995";
        var orderId = "200995202307180240242304uk9399rb9qo";

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.FromUnixTimeMilliseconds(1689649638659);
        _ = fakeSystemClock.CurrentTime.Returns(currentTime);

        var data = "s=4&&orderid=200995202307180240242304uk9399rb9qo";
        var param = "sXIdP1BwVgthYuYHM6eZZmH1GNxEXUWQXuTz9IcmJfqPVPYaUJ6GH3QZ2I7eK98YhvZGcOD1VSQXpijlHPTEiA==";
        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(deskey),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(param);

        var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
        var key = "da5715e12bca5cbd3facb43b9db3e45d";
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
            ["s"] = 104,
            ["d"] = new JsonObject
            {
                ["code"] = 0,
                ["status"] = 0,
                ["money"] = 1
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
            fakeCryptography,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .OrderStatusAsync(
            apiUrl: apiUrl,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            orderId: orderId)
            .ConfigureAwait(false);

        Assert.True(actual);
    }

    [Fact]
    public async void Order_Does_Not_Exist()
    {
        Uri apiUrl = new Uri("https://wc1-api.kewmn686.com/");
        var deskey = "706EB65DD6E6DF4C";
        var md5key = "3E00054B89B31B7A";
        var agentId = "200995";
        var orderId = "200995202307180240242305uk9399rb9qo";

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.FromUnixTimeMilliseconds(1689649638659);
        _ = fakeSystemClock.CurrentTime.Returns(currentTime);

        var data = "s=4&&orderid=200995202307180240242305uk9399rb9qo";
        var param = "sXIdP1BwVgthYuYHM6eZZmH1GNxEXUWQXuTz9IcmJfr+lFULeb8vl3e4SKaqGSf8hvZGcOD1VSQXpijlHPTEiA==";
        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(deskey),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(param);

        var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
        var key = "56d4ec70e0079b7f53549110e8e34e67";
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
            ["s"] = 104,
            ["d"] = new JsonObject
            {
                ["code"] = 0,
                ["status"] = -1,
                ["money"] = 0
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
            fakeCryptography,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .OrderStatusAsync(
            apiUrl: apiUrl,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            orderId: orderId)
            .ConfigureAwait(false);

        Assert.False(actual);
    }
}
