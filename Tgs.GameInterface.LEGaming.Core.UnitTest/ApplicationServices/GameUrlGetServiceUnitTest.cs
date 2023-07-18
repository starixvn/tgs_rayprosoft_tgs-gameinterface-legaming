using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var query = new GameUrlQuery(
            ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
            Deskey: "706EB65DD6E6DF4C",
            MD5key: "3E00054B89B31B7A",
            AgentId: "200995",
            BrandCode: "GPI_HKD",
            PlayerCode: "4uk9399rb9qo",
            IpAddress: "127.0.0.1",
            GameExternalID: "1");

        var fakeClient = Substitute.For<IGameApiClient>();

        var service = new GameUrlGetService(
            fakeClient);

        _ = await service
            .GetAsync(
            query: query)
            .ConfigureAwait(false);

        _ = fakeClient
            .Received(1)
            .LoginAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.IpAddress),
            Arg.Is(query.BrandCode),
            Arg.Is(query.GameExternalID),
            Arg.Any<CancellationToken>());
    }
}
