using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.GameInterface.LEGaming.Core.Models;

namespace Tgs.GameInterface.LEGaming.Core.UnitTest.ApplicationServices;

public class GameWagerQueryServiceUnitTest
{
    [Fact]
    public async Task Player_Wins()
    {
        var query = new GameWagerQuery(
            ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
            Deskey: "706EB65DD6E6DF4C",
            MD5key: "3E00054B89B31B7A",
            AgentId: "200995",
            StartTime: DateTimeOffset.Parse("2023-07-17T16:55:00.000+08:00"),
            EndTime: DateTimeOffset.Parse("2023-07-17T17:00:00.000+08:00"));

        var fakeClient = Substitute.For<IGameRecordApiClient>();

        var infos = new GameRecordInfo[]
        {
            new GameRecordInfo(
                GameID: "686-1689584115-15524392-1",
                Account: "4uk9399rb9qo",
                ServerID: 9101,
                KindID: 910,
                TableID: 182020001,
                ChairID: 0,
                UserCount: 1,
                CellScore: 2.00m,
                AllBet: 2.00m,
                Profit: 2.00m,
                Revenue: 0,
                GameStartTime: DateTimeOffset.Parse("2023-07-17T16:55:15+08:00"),
                GameEndTime: DateTimeOffset.Parse("2023-07-17T16:55:37+08:00"),
                CardValue: "3412001b343718",
                ChannelID: 200995,
                LineCode: "200995_GPI_HKD",
                AdditionalData: new JsonObject
                {
                    ["GameID"] = "686-1689584115-15524392-1",
                    ["Accounts"] = "200995_4uk9399rb9qo",
                    ["ServerID"] = 9101,
                    ["KindID"] = 910,
                    ["TableID"] = 182020001,
                    ["ChairID"] = 0,
                    ["UserCount"] = 1,
                    ["CellScore"] =2.00m,
                    ["AllBet"] = 2.00m,
                    ["Profit"] = 2.00m,
                    ["Revenue"] = 0,
                    ["GameStartTime"] ="2023-07-17 16:55:15",
                    ["GameEndTime"] = "2023-07-17 16:55:37",
                    ["CardValue"] = "3412001b343718",
                    ["ChannelID"] =200995,
                    ["LineCode"] ="200995_GPI_HKD"
                }.ToJsonString()
                )
          };
        _ = fakeClient
            .GameRecordAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            deskey: Arg.Is(query.Deskey),
            md5key: Arg.Is(query.MD5key),
            agentId: Arg.Is(query.AgentId),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(infos.ToAsyncEnumerable());

        var service = new GameWagerQueryService(fakeClient);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(infos.First().GameID, actual.First().ExternalWagerId);
        Assert.Equal(infos.First().KindID.ToString(), actual.First().ExternalGameId);
        Assert.Equal(infos.First().Account, actual.First().PlayerCode);
        Assert.Equal(infos.First().AllBet, actual.First().Bet);
        Assert.Equal(infos.First().CellScore, actual.First().ValidBet);
        Assert.Equal(4.00m, actual.First().Settle);
        Assert.Equal(infos.First().Revenue, actual.First().Commission);
        Assert.Equal(infos.First().Profit, actual.First().WinLoss);
        Assert.Equal(infos.First().GameStartTime, actual.First().ExternalCreatedOn);
        Assert.Equal(infos.First().GameEndTime, actual.First().ExternalClosedOn);
        Assert.Equal(infos.First().AdditionalData, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Player_Loses()
    {
        var query = new GameWagerQuery(
            ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
            Deskey: "706EB65DD6E6DF4C",
            MD5key: "3E00054B89B31B7A",
            AgentId: "200995",
            StartTime: DateTimeOffset.Parse("2023-07-17T16:55:00.000+08:00"),
            EndTime: DateTimeOffset.Parse("2023-07-17T17:00:00.000+08:00"));

        var fakeClient = Substitute.For<IGameRecordApiClient>();

        var infos = new GameRecordInfo[]
        {
            new GameRecordInfo(
                GameID: "686-1689584140-15524700-1",
                Account: "4uk9399rb9qo",
                ServerID: 9101,
                KindID: 910,
                TableID: 182020001,
                ChairID: 0,
                UserCount: 1,
                CellScore: 2.00m,
                AllBet: 2.00m,
                Profit: -2.00m,
                Revenue: 0,
                GameStartTime: DateTimeOffset.Parse("2023-07-17T16:55:40+08:00"),
                GameEndTime: DateTimeOffset.Parse("2023-07-17T16:56:02+08:00"),
                CardValue: "11150019163328",
                ChannelID: 200995,
                LineCode: "200995_GPI_HKD",
                AdditionalData: new JsonObject
                {
                    ["GameID"] = "686-1689584140-15524700-1",
                    ["Accounts"] = "200995_4uk9399rb9qo",
                    ["ServerID"] = 9101,
                    ["KindID"] = 910,
                    ["TableID"] = 182020001,
                    ["ChairID"] = 0,
                    ["UserCount"] = 1,
                    ["CellScore"] =2.00m,
                    ["AllBet"] = 2.00m,
                    ["Profit"] = -2.00m,
                    ["Revenue"] = 0,
                    ["GameStartTime"] ="2023-07-17 16:55:40",
                    ["GameEndTime"] = "2023-07-17 16:56:02",
                    ["CardValue"] = "11150019163328",
                    ["ChannelID"] =200995,
                    ["LineCode"] ="200995_GPI_HKD"
                }.ToJsonString()
                )
          };
        _ = fakeClient
            .GameRecordAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            deskey: Arg.Is(query.Deskey),
            md5key: Arg.Is(query.MD5key),
            agentId: Arg.Is(query.AgentId),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(infos.ToAsyncEnumerable());

        var service = new GameWagerQueryService(
            fakeClient);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(infos.First().GameID, actual.First().ExternalWagerId);
        Assert.Equal(infos.First().KindID.ToString(), actual.First().ExternalGameId);
        Assert.Equal(infos.First().Account, actual.First().PlayerCode);
        Assert.Equal(infos.First().AllBet, actual.First().Bet);
        Assert.Equal(infos.First().CellScore, actual.First().ValidBet);
        Assert.Equal(0, actual.First().Settle);
        Assert.Equal(infos.First().Revenue, actual.First().Commission);
        Assert.Equal(infos.First().Profit, actual.First().WinLoss);
        Assert.Equal(infos.First().GameStartTime, actual.First().ExternalCreatedOn);
        Assert.Equal(infos.First().GameEndTime, actual.First().ExternalClosedOn);
        Assert.Equal(infos.First().AdditionalData, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Player_Versus_Banker_Versus_Player()
    {
        var query = new GameWagerQuery(
            ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
            Deskey: "706EB65DD6E6DF4C",
            MD5key: "3E00054B89B31B7A",
            AgentId: "200995",
            StartTime: DateTimeOffset.Parse("2023-07-18T11:59:00.000+08:00"),
            EndTime: DateTimeOffset.Parse("2023-07-18T12:09:00.000+08:00"));

        var fakeClient = Substitute.For<IGameRecordApiClient>();

        var infos = new GameRecordInfo[]
        {
            new GameRecordInfo(
                GameID: "686-1689652975-19588451-1",
                Account: "4uk9399rb9qo",
                ServerID: 9101,
                KindID: 910,
                TableID: 182020001,
                ChairID: 0,
                UserCount: 1,
                CellScore: 0.00m,
                AllBet: 4.00m,
                Profit: 0.00m,
                Revenue: 0,
                GameStartTime: DateTimeOffset.Parse("2023-07-18T12:02:55+08:00"),
                GameEndTime: DateTimeOffset.Parse("2023-07-18T12:03:17+08:00"),
                CardValue: "183900112a0d1",
                ChannelID: 200995,
                LineCode: "200995_GPI_HKD",
                AdditionalData: new JsonObject
                {
                    ["GameID"] = "686-1689652975-19588451-1",
                    ["Accounts"] = "200995_4uk9399rb9qo",
                    ["ServerID"] = 9101,
                    ["KindID"] = 910,
                    ["TableID"] = 182020001,
                    ["ChairID"] = 0,
                    ["UserCount"] = 1,
                    ["CellScore"] =0.00m,
                    ["AllBet"] = 4.00m,
                    ["Profit"] = 0.00m,
                    ["Revenue"] = 0,
                    ["GameStartTime"] ="2023-07-18 12:02:55",
                    ["GameEndTime"] = "2023-07-18 12:03:17",
                    ["CardValue"] = "183900112a0d1",
                    ["ChannelID"] =200995,
                    ["LineCode"] ="200995_GPI_HKD"
                }.ToJsonString()
                )
          };
        _ = fakeClient
            .GameRecordAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            deskey: Arg.Is(query.Deskey),
            md5key: Arg.Is(query.MD5key),
            agentId: Arg.Is(query.AgentId),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(infos.ToAsyncEnumerable());

        var service = new GameWagerQueryService(
            fakeClient);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(infos.First().GameID, actual.First().ExternalWagerId);
        Assert.Equal(infos.First().KindID.ToString(), actual.First().ExternalGameId);
        Assert.Equal(infos.First().Account, actual.First().PlayerCode);
        Assert.Equal(infos.First().AllBet, actual.First().Bet);
        Assert.Equal(infos.First().CellScore, actual.First().ValidBet);
        Assert.Equal(4m, actual.First().Settle);
        Assert.Equal(infos.First().Revenue, actual.First().Commission);
        Assert.Equal(infos.First().Profit, actual.First().WinLoss);
        Assert.Equal(infos.First().GameStartTime, actual.First().ExternalCreatedOn);
        Assert.Equal(infos.First().GameEndTime, actual.First().ExternalClosedOn);
        Assert.Equal(infos.First().AdditionalData, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Player_Versus_Banker_Versus_Player_Versus_Tie()
    {
        var query = new GameWagerQuery(
            ApiUrl: new Uri("https://wc1-api.kewmn686.com/"),
            Deskey: "706EB65DD6E6DF4C",
            MD5key: "3E00054B89B31B7A",
            AgentId: "200995",
            StartTime: DateTimeOffset.Parse("2023-07-18T11:59:00.000+08:00"),
            EndTime: DateTimeOffset.Parse("2023-07-18T12:09:00.000+08:00"));

        var fakeClient = Substitute.For<IGameRecordApiClient>();

        var infos = new GameRecordInfo[]
        {
            new GameRecordInfo(
                GameID: "686-1689653000-19588758-1",
                Account: "4uk9399rb9qo",
                ServerID: 9101,
                KindID: 910,
                TableID: 182020001,
                ChairID: 0,
                UserCount: 1,
                CellScore: 2.00m,
                AllBet: 6.00m,
                Profit: -2.00m,
                Revenue: 0,
                GameStartTime: DateTimeOffset.Parse("2023-07-18T12:03:20+08:00"),
                GameEndTime: DateTimeOffset.Parse("2023-07-18T12:03:44+08:00"),
                CardValue: "0605372c3d011",
                ChannelID: 200995,
                LineCode: "200995_GPI_HKD",
                AdditionalData: new JsonObject
                {
                    ["GameID"] = "686-1689653000-19588758-1",
                    ["Accounts"] = "200995_4uk9399rb9qo",
                    ["ServerID"] = 9101,
                    ["KindID"] = 910,
                    ["TableID"] = 182020001,
                    ["ChairID"] = 0,
                    ["UserCount"] = 1,
                    ["CellScore"] =2.00m,
                    ["AllBet"] = 6.00m,
                    ["Profit"] = -2.00m,
                    ["Revenue"] = 0,
                    ["GameStartTime"] ="2023-07-18 12:03:20",
                    ["GameEndTime"] = "2023-07-18 12:03:44",
                    ["CardValue"] = "0605372c3d011",
                    ["ChannelID"] =200995,
                    ["LineCode"] ="200995_GPI_HKD"
                }.ToJsonString()
                )
          };
        _ = fakeClient
            .GameRecordAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            deskey: Arg.Is(query.Deskey),
            md5key: Arg.Is(query.MD5key),
            agentId: Arg.Is(query.AgentId),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(infos.ToAsyncEnumerable());

        var service = new GameWagerQueryService(
            fakeClient);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(infos.First().GameID, actual.First().ExternalWagerId);
        Assert.Equal(infos.First().KindID.ToString(), actual.First().ExternalGameId);
        Assert.Equal(infos.First().Account, actual.First().PlayerCode);
        Assert.Equal(infos.First().AllBet, actual.First().Bet);
        Assert.Equal(infos.First().CellScore, actual.First().ValidBet);
        Assert.Equal(4m, actual.First().Settle);
        Assert.Equal(infos.First().Revenue, actual.First().Commission);
        Assert.Equal(infos.First().Profit, actual.First().WinLoss);
        Assert.Equal(infos.First().GameStartTime, actual.First().ExternalCreatedOn);
        Assert.Equal(infos.First().GameEndTime, actual.First().ExternalClosedOn);
        Assert.Equal(infos.First().AdditionalData, actual.First().AdditionalData);
    }
}
