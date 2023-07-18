using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.GameInterface.LEGaming.Core.Models;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
            Deskey: "706EB65DD6E6DF4C",
            MD5key: "3E00054B89B31B7A",
            AgentId: "200995",
            PlayerCode: "4uk9399rb9qo");

        var fakeClient = Substitute.For<IGameApiClient>();

        var info = new BalanceInfo(
            TotalMoney: 100,
            FreeMoney: 98);
        _ = fakeClient
            .BalanceAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Deskey),
            Arg.Is(query.MD5key),
            Arg.Is(query.AgentId),
            Arg.Is(query.PlayerCode),
            Arg.Any<CancellationToken>())
            .Returns(info);

        var service = new PlayerBalanceGetService(
            fakeClient);

        var actual = await service
            .GetAsync(
            query: query)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(100, actual.Balance);
        Assert.Equal(98, actual.FreeBalance);
        Assert.Equal(2, actual.LockBalance);
    }
}
