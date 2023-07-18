using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest;

public class GameRecordUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        Uri apiUrl = new Uri("https://wc1-record.kewmn686.com/");
        var deskey = "706EB65DD6E6DF4C";
        var md5key = "3E00054B89B31B7A";
        var agentId = "200995";
        var startTime = DateTimeOffset.Parse("2023-07-17T16:55:00+08:00");
        var endTime = DateTimeOffset.Parse("2023-07-17T17:00:00+08:00");

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.FromUnixTimeMilliseconds(1689586504832);
        _ = fakeSystemClock.CurrentTime.Returns(currentTime);

        var data = "s=6&startTime=1689584100000&endTime=1689584400000";
        var param = "IvzEoNbMqRigzbXIyXr57L0CX7PwyeoP0ZKxXnUD7eWfOeNawl6V2011CvUs4FYPQaUG2zsAqsctjCu/8MMYqg==";
        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(deskey),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(param);

        var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
        var key = "dd8d6b1f14b3060ea47ec66285b442ef";
        _ = fakeCryptography.GetHash(
            data: Arg.Is(dataMD5))
            .Returns(key);

        var uriBuilder = new UriBuilder(new Uri(apiUrl, "/getRecordHandle"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["agent"] = agentId;
        query["timestamp"] = currentTime.ToUnixTimeMilliseconds().ToString();
        query["param"] = param;
        query["key"] = key;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["m"] = "/getRecordHandle",
            ["s"] = 106,
            ["d"] = new JsonObject
            {
                ["code"] = 0,
                ["start"] = 1689584100000,
                ["end"] = 1689584400000,
                ["count"] = 2,
                ["list"] = new JsonObject
                {
                    ["GameID"] = new JsonArray {
                        "686-1689584115-15524392-1",
                        "686-1689584140-15524700-1"
                    },
                    ["Accounts"] = new JsonArray {
                        "200995_4uk9399rb9qo",
                        "200995_4uk9399rb9qo"
                    },
                    ["ServerID"] = new JsonArray {
                        9101,
                        9101
                    },
                    ["KindID"] = new JsonArray {
                        910,
                        910
                    },
                    ["TableID"] = new JsonArray {
                        182020001,
                        182020001
                    },
                    ["ChairID"] = new JsonArray {
                        0,
                        0
                    },
                    ["UserCount"] = new JsonArray {
                        1,
                        1
                    },
                    ["CellScore"] = new JsonArray {
                        "2.00",
                        "2.00"
                    },
                    ["AllBet"] = new JsonArray {
                        "2.00",
                        "2.00"
                    },
                    ["Profit"] = new JsonArray {
                        "2.00",
                        "-2.00"
                    },
                    ["Revenue"] = new JsonArray {
                        "0.00",
                        "0.00"
                    },
                    ["GameStartTime"] = new JsonArray {
                        "2023-07-17 16:55:15",
                        "2023-07-17 16:55:40"
                    },
                    ["GameEndTime"] = new JsonArray {
                        "2023-07-17 16:55:37",
                        "2023-07-17 16:56:02"
                    },
                    ["CardValue"] = new JsonArray {
                        "3412001b343718",
                        "11150019163328"
                    },
                    ["ChannelID"] = new JsonArray {
                        200995,
                        200995
                    },
                    ["LineCode"] = new JsonArray {
                        "200995_GPI_HKD",
                        "200995_GPI_HKD"
                    }
                }
            }
        };

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, uriBuilder.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameRecordApiClient>.Instance;

        var client = new GameRecordApiClient(
            fakeLog,
            fakeCryptography,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .GameRecordAsync(
            apiUrl: apiUrl,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            startTime: startTime,
            endTime: endTime,
            retrySeconds: 1)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal("686-1689584115-15524392-1", actual.First().GameID);
        Assert.Equal("4uk9399rb9qo", actual.First().Account);
        Assert.Equal(9101, actual.First().ServerID);
        Assert.Equal(910, actual.First().KindID);
        Assert.Equal(182020001, actual.First().TableID);
        Assert.Equal(0, actual.First().ChairID);
        Assert.Equal(1, actual.First().UserCount);
        Assert.Equal(2.00m, actual.First().CellScore);
        Assert.Equal(2.00m, actual.First().AllBet);
        Assert.Equal(2.00m, actual.First().Profit);
        Assert.Equal(0.00m, actual.First().Revenue);
        Assert.Equal(DateTimeOffset.Parse("2023-07-17T16:55:15+08:00"), actual.First().GameStartTime);
        Assert.Equal(DateTimeOffset.Parse("2023-07-17T16:55:37+08:00"), actual.First().GameEndTime);
        Assert.Equal("3412001b343718", actual.First().CardValue);
        Assert.Equal(200995, actual.First().ChannelID);
        Assert.Equal("200995_GPI_HKD", actual.First().LineCode);
    }

    [Fact]
    public async void Data_Does_Not_Exist_Code_16()
    {
        Uri apiUrl = new Uri("https://wc1-record.kewmn686.com/");
        var deskey = "706EB65DD6E6DF4C";
        var md5key = "3E00054B89B31B7A";
        var agentId = "200995";
        var startTime = DateTimeOffset.Parse("2023-07-17T16:55:00+08:00");
        var endTime = DateTimeOffset.Parse("2023-07-17T17:00:00+08:00");

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.FromUnixTimeMilliseconds(1689586504832);
        _ = fakeSystemClock.CurrentTime.Returns(currentTime);

        var data = "s=6&startTime=1689584100000&endTime=1689584400000";
        var param = "IvzEoNbMqRigzbXIyXr57L0CX7PwyeoP0ZKxXnUD7eWfOeNawl6V2011CvUs4FYPQaUG2zsAqsctjCu/8MMYqg==";
        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(deskey),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(param);

        var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
        var key = "dd8d6b1f14b3060ea47ec66285b442ef";
        _ = fakeCryptography.GetHash(
            data: Arg.Is(dataMD5))
            .Returns(key);

        var uriBuilder = new UriBuilder(new Uri(apiUrl, "/getRecordHandle"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["agent"] = agentId;
        query["timestamp"] = currentTime.ToUnixTimeMilliseconds().ToString();
        query["param"] = param;
        query["key"] = key;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["m"] = "/getRecordHandle",
            ["s"] = 106,
            ["d"] = new JsonObject
            {
                ["code"] = 16,
                ["start"] = 1689584100000,
                ["end"] = 1689584400000,
            }
        };

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, uriBuilder.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameRecordApiClient>.Instance;

        var client = new GameRecordApiClient(
            fakeLog,
            fakeCryptography,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .GameRecordAsync(
            apiUrl: apiUrl,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            startTime: startTime,
            endTime: endTime,
            retrySeconds: 1)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }

    [Fact]
    public async void Pulling_Orders_Too_Frequently_Coode_43()
    {
        Uri apiUrl = new Uri("https://wc1-record.kewmn686.com/");
        var deskey = "706EB65DD6E6DF4C";
        var md5key = "3E00054B89B31B7A";
        var agentId = "200995";
        var startTime = DateTimeOffset.Parse("2023-07-17T16:55:00+08:00");
        var endTime = DateTimeOffset.Parse("2023-07-17T17:00:00+08:00");

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.FromUnixTimeMilliseconds(1689586504832);
        _ = fakeSystemClock.CurrentTime.Returns(currentTime);

        var data = "s=6&startTime=1689584100000&endTime=1689584400000";
        var param = "IvzEoNbMqRigzbXIyXr57L0CX7PwyeoP0ZKxXnUD7eWfOeNawl6V2011CvUs4FYPQaUG2zsAqsctjCu/8MMYqg==";
        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(deskey),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(param);

        var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
        var key = "dd8d6b1f14b3060ea47ec66285b442ef";
        _ = fakeCryptography.GetHash(
            data: Arg.Is(dataMD5))
            .Returns(key);

        var uriBuilder = new UriBuilder(new Uri(apiUrl, "/getRecordHandle"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["agent"] = agentId;
        query["timestamp"] = currentTime.ToUnixTimeMilliseconds().ToString();
        query["param"] = param;
        query["key"] = key;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["m"] = "/getRecordHandle",
            ["s"] = 106,
            ["d"] = new JsonObject
            {
                ["code"] = 43
            }
        };

        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Get, uriBuilder.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameRecordApiClient>.Instance;

        var client = new GameRecordApiClient(
            fakeLog,
            fakeCryptography,
            fakeSystemClock,
            fakeHttpClient);

        _ = await Assert.ThrowsAsync<HttpRequestException>(
            () => client
            .GameRecordAsync(
                apiUrl: apiUrl,
                deskey: deskey,
                md5key: md5key,
                agentId: agentId,
                startTime: startTime,
                endTime: endTime,
                retrySeconds: 1)
            .ToArrayAsync()
            .AsTask())
            .ConfigureAwait(false);
    }
}
