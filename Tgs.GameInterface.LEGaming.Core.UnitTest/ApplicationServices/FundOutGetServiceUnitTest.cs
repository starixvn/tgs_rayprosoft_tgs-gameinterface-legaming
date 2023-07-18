using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var query = new FundOutQuery(
            ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
            Deskey: "706EB65DD6E6DF4C",
            MD5key: "3E00054B89B31B7A",
            AgentId: "200995",
            PlayerCode: "4uk9399rb9qo",
            Amount: 1.0m);

        var fakeClient = Substitute.For<IGameApiClient>();

        var service = new FundOutGetService(
            fakeClient);

        _ = await service
            .GetAsync(
            query: query)
            .ConfigureAwait(false);

        _ = fakeClient
            .Received(1)
            .DepositAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.Amount),
            Arg.Any<CancellationToken>());
    }
}
