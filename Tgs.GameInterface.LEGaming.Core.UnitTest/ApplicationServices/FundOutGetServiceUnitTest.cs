using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.GameInterface.LEGaming.Core.Models;

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
            OrderId: "200995202307180240242304uk9399rb9qo",
            Amount: 1.0m);

        var fakeClient = Substitute.For<IGameApiClient>();

        _ = fakeClient
            .DepositAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.OrderId),
            Arg.Is(query.Amount),
            Arg.Any<CancellationToken>())
            .Returns(new ValueTask<DepositResp>(
                new DepositResp(Resp: "{\"m\":\"/channelHandle\",\"s\":102,\"d\":{\"code\":0,\"money\":\"1.00\"}}")));

        var service = new FundOutGetService(
            fakeClient);

        var actual = await service.GetAsync(query: query);

        Assert.Equal("{\"m\":\"/channelHandle\",\"s\":102,\"d\":{\"code\":0,\"money\":\"1.00\"}}", actual.Resp);
    }
}
