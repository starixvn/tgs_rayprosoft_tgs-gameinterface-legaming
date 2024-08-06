using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.LEGaming.Protos;

namespace Tgs.GameInterface.LEGaming.Client;

internal class GameInterfaceLEGamingClient(
	GameInterfaceLEGaming.GameInterfaceLEGamingClient client) : IGameInterfaceLEGamingClient
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client
			.CheckingServiceAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string deskey, string md5key, string agentId, string agentCurrency, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			Deskey = deskey,
			MD5Key = md5key,
			AgentId = agentId,
			AgentCurrency = agentCurrency,
			PlayerCode = playerCode,
			OrderId = orderId,
			Amount = amount
		};

		return await client.FundInAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string deskey, string md5key, string agentId, string agentCurrency, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			Deskey = deskey,
			MD5Key = md5key,
			AgentId = agentId,
			AgentCurrency = agentCurrency,
			PlayerCode = playerCode,
			OrderId = orderId,
			Amount = amount
		};

		return await client.FundOutAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string deskey, string md5key, string agentId, string orderId, CancellationToken cancellationToken = default)
	{
		var request = new FundStatusRequest
		{
			ApiUrl = apiUrl.ToString(),
			Deskey = deskey,
			MD5Key = md5key,
			AgentId = agentId,
			OrderId = orderId,
		};

		var reply = await client
	   .FundStatusAsync(
		   request: request,
		   cancellationToken: cancellationToken)
	   .ConfigureAwait(false);

		return reply.State;
	}

	public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string deskey, string md5key, string agentId, DateTimeOffset startTime, DateTimeOffset endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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

		var reply = await client.GameRecordAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		foreach (var info in reply.Items)
		{
			yield return info;
		}
	}

	public async ValueTask<string> GameUrlAsync(Uri apiUrl, string deskey, string md5key, string agentId, string agentCurrency, string brandCode, string playerCode, string ipAddress, string externalGameId, CancellationToken cancellationToken = default)
	{
		var request = new GameUrlRequest
		{
			ApiUrl = apiUrl.ToString(),
			Deskey = deskey,
			MD5Key = md5key,
			AgentId = agentId,
			AgentCurrency = agentCurrency,
			BrandCode = brandCode,
			PlayerCode = playerCode,
			IpAddress = ipAddress,
			ExternalGameId = externalGameId
		};

		var reply = await client
	   .GameUrlAsync(
		   request: request,
		   cancellationToken: cancellationToken)
	   .ConfigureAwait(false);

		return reply.GameUrl.ToString();
	}

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string deskey, string md5key, string agentId, string agentCurrency, string playerCode, CancellationToken cancellationToken = default)
	{
		var request = new PlayerBalanceRequest
		{
			ApiUrl = apiUrl.ToString(),
			Deskey = deskey,
			MD5Key = md5key,
			AgentId = agentId,
			AgentCurrency = agentCurrency,
			PlayerCode = playerCode
		};

		return await client
	   .PlayerBalanceAsync(
		   request: request,
		   cancellationToken: cancellationToken)
	   .ConfigureAwait(false);
	}

	public async ValueTask PlayerLogoutAsync(Uri apiUrl, string deskey, string md5key, string agentId, string playerCode, CancellationToken cancellationToken = default)
	{
		var request = new PlayerLogoutRequest
		{
			ApiUrl = apiUrl.ToString(),
			Deskey = deskey,
			MD5Key = md5key,
			AgentId = agentId,
			PlayerCode = playerCode
		};

		_ = await client
		.PlayerLogoutAsync(
			request: request,
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);
	}

	public async ValueTask<AgentBalanceReply> AgentBalanceAsync(Uri apiUrl, string deskey, string md5key, string agentId, CancellationToken cancellationToken = default)
	{
		var request = new AgentBalanceRequest
		{
			ApiUrl = apiUrl.ToString(),
			Deskey = deskey,
			MD5Key = md5key,
			AgentId = agentId
		};

		return await client.AgentBalanceAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}
