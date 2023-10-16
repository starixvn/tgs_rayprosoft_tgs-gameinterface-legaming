using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest;

public class LoginUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        var apiUrl = new Uri("https://wc1-api.kewmn686.com/");
        var deskey = "706EB65DD6E6DF4C";
        var md5key = "3E00054B89B31B7A";
        var agentId = "200995";
        var account = "4uk9399rb9qo";
        var ipAddress = "127.0.0.1";
        var lineCode = "GPI_HKD";
        var kindID = "0";

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.Parse("2023-07-18T03:20:28.852+00:00");
        _ = fakeSystemClock.CurrentTime.Returns(currentTime);

        var data = "s=0&account=4uk9399rb9qo&money=0&orderid=200995202307180320288524uk9399rb9qo&ip=127.0.0.1&lineCode=GPI_HKD&KindID=0";
        var param = "HQN9/G1xjatVFubjmWOt1qOD/F6kv2maQn09UTu/K8Wqcq16IBeEvLm3txrRJ8R8sgrPlCeNHHXMTN1SOIIQkp0w9Kig2V9UKrPRprh4M+X/NBrEYMHU+lDW4RsR/R/xgvAreKAny/sD8BhoOHoHr0UTotxZ5BoA9bhTO2jD/Tc=";
        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(deskey),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(param);

        var dataMD5 = $"{agentId}{currentTime.ToUnixTimeMilliseconds()}{md5key}";
        var key = "3971af284eac5bc9bf844b08a6155fd8";
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
            ["s"] = 100,
            ["d"] = new JsonObject
            {
                ["code"] = 0,
                ["url"] = "https://wc1-g-h5.kewmn686.com/",
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
            .LoginAsync(
            apiUrl: apiUrl,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            account: account,
            ipAddress: ipAddress,
            lineCode: lineCode,
            kindID: kindID);

        Assert.NotNull(actual);
        Assert.Equal("https://wc1-g-h5.kewmn686.com/", actual);
    }
}
