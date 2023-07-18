using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.LEGaming.Protos;

namespace Tgs.GameInterface.LEGaming.Client;

internal class GameInterfaceLEGamingClient : IGameInterfaceLEGamingClient
{
    private readonly GameInterfaceLEGaming.GameInterfaceLEGamingClient _client;

    public GameInterfaceLEGamingClient(GameInterfaceLEGaming.GameInterfaceLEGamingClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<string> FundInAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string playerCode,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest
        {
            ApiUrl = apiUrl.ToString(),
            Deskey = deskey,
            MD5Key = md5key,
            AgentId = agentId,
            PlayerCode = playerCode,
            Amount = amount
        };

        var reply = await _client
       .FundInAsync(
           request: request,
           cancellationToken: cancellationToken)
       .ConfigureAwait(false);

        return reply.OrderId;
    }

    public async ValueTask<string> FundOutAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string playerCode,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest
        {
            ApiUrl = apiUrl.ToString(),
            Deskey = deskey,
            MD5Key = md5key,
            AgentId = agentId,
            PlayerCode = playerCode,
            Amount = amount
        };

        var reply = await _client
       .FundOutAsync(
           request: request,
           cancellationToken: cancellationToken)
       .ConfigureAwait(false);

        return reply.OrderId;
    }

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string orderId,
        CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest
        {
            ApiUrl = apiUrl.ToString(),
            Deskey = deskey,
            MD5Key = md5key,
            AgentId = agentId,
            OrderId = orderId,
        };

        var reply = await _client
       .FundStatusAsync(
           request: request,
           cancellationToken: cancellationToken)
       .ConfigureAwait(false);

        return reply.State;
    }

    public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameRecordRequest
        {
            ApiUrl = apiUrl.ToString(),
            Deskey = deskey,
            MD5Key = md5key,
            AgentId = agentId,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await _client
       .GameRecordAsync(
           request: request,
           cancellationToken: cancellationToken)
       .ConfigureAwait(false);

        var gameRecords = reply.Items
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return gameRecord;
        }
    }

    public async ValueTask<string> GameUrlAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string brandCode,
        string playerCode,
        string ipAddress,
        string gameExternalID,
        CancellationToken cancellationToken = default)
    {
        var request = new GameUrlRequest
        {
            ApiUrl = apiUrl.ToString(),
            Deskey = deskey,
            MD5Key = md5key,
            AgentId = agentId,
            BrandCode = brandCode,
            PlayerCode = playerCode,
            IpAddress = ipAddress,
            GameExternalId = gameExternalID
        };

        var reply = await _client
       .GameUrlAsync(
           request: request,
           cancellationToken: cancellationToken)
       .ConfigureAwait(false);

        return reply.GameUrl.ToString();
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerBalanceRequest
        {
            ApiUrl = apiUrl.ToString(),
            Deskey = deskey,
            MD5Key = md5key,
            AgentId = agentId,
            PlayerCode = playerCode
        };

        return await _client
       .PlayerBalanceAsync(
           request: request,
           cancellationToken: cancellationToken)
       .ConfigureAwait(false);
    }

    public async ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerLogoutRequest
        {
            ApiUrl = apiUrl.ToString(),
            Deskey = deskey,
            MD5Key = md5key,
            AgentId = agentId,
            PlayerCode = playerCode
        };

        _ = await _client
        .PlayerLogoutAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
    }
}
