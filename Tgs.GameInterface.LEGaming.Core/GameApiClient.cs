using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.LEGaming.Core.Exceptions;
using Tgs.GameInterface.LEGaming.Core.Extensions;
using Tgs.GameInterface.LEGaming.Core.Models;

namespace Tgs.GameInterface.LEGaming.Core;

internal class GameApiClient : IGameApiClient
{
    private readonly ILogger<GameApiClient> _logger;
    private readonly IHashCryptography _cryptography;
    private readonly ISystemClock _systemClock;
    private readonly HttpClient _client;

    public GameApiClient(
        ILogger<GameApiClient> logger,
        IHashCryptography cryptography,
        ISystemClock systemClock,
        HttpClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cryptography = cryptography ?? throw new ArgumentNullException(nameof(cryptography));
        _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<BalanceInfo> BalanceAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string account,
        CancellationToken cancellationToken = default)
    {
        var subTypes = SubTypes.Balance;
        var data = $"s={(int)subTypes}&account={account}";
        var jsonNode = await GetAsync(
            apiUrl: apiUrl,
            types: subTypes,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            data: data,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var dataResponse = jsonNode["d"]!;
        return dataResponse["code"]!.GetValue<int>() != 0
            ? throw new ReturnCodeException(dataResponse["code"]!.GetValue<int>())
            : new BalanceInfo(
            dataResponse["totalMoney"]!.GetValue<decimal>(),
            dataResponse["freeMoney"]!.GetValue<decimal>());
    }

    public async ValueTask<string> DepositAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string account,
        decimal money,
        CancellationToken cancellationToken = default)
    {
        var subTypes = SubTypes.Deposit;
        var orderId = $"{agentId}{_systemClock.CurrentTime:yyyyMMddHHmmssfff}{account}";
        var data = $"s={(int)subTypes}&account={account}&money={money}&orderid={orderId}";
        var jsonNode = await GetAsync(
            apiUrl: apiUrl,
            types: subTypes,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            data: data,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var dataResponse = jsonNode["d"]!;
        return dataResponse["code"]!.GetValue<int>() != 0
            ? throw new ReturnCodeException(dataResponse["code"]!.GetValue<int>())
            : orderId;
    }

    public async ValueTask<string> LoginAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string account,
        string ipAddress,
        string lineCode,
        string gameExternalID,
        CancellationToken cancellationToken = default)

    {
        var subTypes = SubTypes.Login;
        var orderId = $"{agentId}{_systemClock.CurrentTime:yyyyMMddHHmmssfff}{account}";
        var data = $"s={(int)subTypes}&account={account}&money=0&orderid={orderId}&ip={ipAddress}&lineCode={lineCode}&KindID={gameExternalID}";
        var jsonNode = await GetAsync(
            apiUrl: apiUrl,
            types: subTypes,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            data: data,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var dataResponse = jsonNode["d"]!;
        return dataResponse["code"]!.GetValue<int>() != 0
            ? throw new ReturnCodeException(dataResponse["code"]!.GetValue<int>())
            : dataResponse["url"]!.GetValue<string>();
    }

    public async ValueTask LogoutAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string account,
        CancellationToken cancellationToken = default)
    {
        var subTypes = SubTypes.Logout;
        var data = $"s={(int)subTypes}&account={account}";
        var jsonNode = await GetAsync(
            apiUrl: apiUrl,
            types: subTypes,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            data: data,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var dataResponse = jsonNode["d"]!;
        if (dataResponse["code"]!.GetValue<int>() != 0)
        {
            throw new ReturnCodeException(dataResponse["code"]!.GetValue<int>());
        }
    }

    public async ValueTask<bool> OrderStatusAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string orderId,
        CancellationToken cancellationToken = default)

    {
        var subTypes = SubTypes.OrderStatus;
        var data = $"s={(int)subTypes}&&orderid={orderId}";
        var jsonNode = await GetAsync(
            apiUrl: apiUrl,
            types: subTypes,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            data: data,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var dataResponse = jsonNode["d"]!;
        return dataResponse["code"]!.GetValue<int>() != 0
            ? throw new ReturnCodeException(dataResponse["code"]!.GetValue<int>())
            : dataResponse["status"]!.GetValue<int>() is not (-1) and not 0 and not 2
                ? throw new Exception($"訂單狀態請聯絡客服協助處理(status:{dataResponse["status"]!.GetValue<int>()})")
                : dataResponse["status"]!.GetValue<int>() == 0;
    }

    public async ValueTask<string> WithdrawalAsync(
        Uri apiUrl,
        string deskey,
        string md5key,
        string agentId,
        string account,
        decimal money,
        CancellationToken cancellationToken = default)

    {
        var subTypes = SubTypes.Withdrawal;
        var orderId = $"{agentId}{_systemClock.CurrentTime:yyyyMMddHHmmssfff}{account}";
        var data = $"s={(int)subTypes}&account={account}&money={money}&orderid={orderId}";
        var jsonNode = await GetAsync(
            apiUrl: apiUrl,
            types: subTypes,
            deskey: deskey,
            md5key: md5key,
            agentId: agentId,
            data: data,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var dataResponse = jsonNode["d"]!;
        return dataResponse["code"]!.GetValue<int>() != 0
            ? throw new ReturnCodeException(dataResponse["code"]!.GetValue<int>())
            : orderId;
    }

    private async ValueTask<JsonNode> GetAsync(
        Uri apiUrl,
        SubTypes types,
        string deskey,
        string md5key,
        string agentId,
        string data,
        CancellationToken cancellationToken = default)
    {
        var timestamp = _systemClock.CurrentTime.ToUnixTimeMilliseconds();
        var uriBuilder = new UriBuilder(new Uri(apiUrl, "/channelHandle"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["agent"] = agentId;
        query["timestamp"] = timestamp.ToString();
        query["param"] = await _cryptography
            .GetAesEncryptAsync(
            data: data,
            secretKey: deskey,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        query["key"] = _cryptography.GetHash($"{agentId}{timestamp}{md5key}");
        uriBuilder.Query = query.ToString();

        var logPrefix = $"{nameof(GameApiClient)}.{types}";

        var sw = Stopwatch.StartNew();

        var httpResponseMessage = await _client
            .GetAsync(
                requestUri: uriBuilder.ToString(),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        sw.Stop();

        var responseMessage = default(string);
        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr
                .ReadToEndAsync(
                    cancellationToken: cancellationToken)
                ?? string.Empty;
        }

        var logEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = data.NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        _logger.LogInformation("{logEvent}", JsonSerializer.Serialize(logEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return string.IsNullOrEmpty(responseMessage)
            ? throw new Exception("Game provider response content is empty.")
            : JsonNode.Parse(responseMessage)!;
    }
}
