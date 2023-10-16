using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_ProcessAsync()
    {
        var command = new PlayerLogoutCommand(
            ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
            Deskey: "706EB65DD6E6DF4C",
            MD5key: "3E00054B89B31B7A",
            AgentId: "200995",
            PlayerCode: "4uk9399rb9qo");

        var fakeClient = Substitute.For<IGameApiClient>();

        var service = new PlayerLogoutCommandService(fakeClient);

        await service.ExecuteAsync(command: command);

        _ = fakeClient
            .Received(1)
            .LogoutAsync(
            Arg.Is(command.ApiUrl),
            Arg.Is(command.Deskey),
            Arg.Is(command.MD5key),
            Arg.Is(command.AgentId),
            Arg.Is(command.PlayerCode),
            Arg.Any<CancellationToken>());
    }
}
