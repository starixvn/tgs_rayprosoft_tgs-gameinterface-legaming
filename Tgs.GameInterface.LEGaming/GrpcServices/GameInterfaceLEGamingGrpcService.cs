using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.GameInterface.LEGaming.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.GrpcServices;

public class GameInterfaceLEGamingGrpcService(
	ILogger<GameInterfaceLEGamingGrpcService> logger,
	IGetService<FundInQuery, FundResult> fundInService,
	IGetService<FundOutQuery, FundResult> fundOutService,
	IGetService<FundStatusQuery, bool> fundStatusService,
	IQueryService<GameRecordQuery, GameRecordResult> gameWagerService,
	IGetService<GameUrlQuery, string> gameUrlService,
	IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
	ICommandService<PlayerLogoutCommand> PlayerLogoutService,
	IGetService<AgentBalanceQuery, AgentBalanceResult> agentBalanceService) : GameInterfaceLEGaming.GameInterfaceLEGamingBase
{
	public override async Task<AgentBalanceReply> AgentBalance(AgentBalanceRequest request, ServerCallContext context)
	{
		var result = await agentBalanceService.GetAsync(
			query: new AgentBalanceQuery
			{
				ApiUrl = new(request.ApiUrl),
				Deskey = request.Deskey,
				MD5key = request.MD5Key,
				AgentId = request.AgentId
			},
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new AgentBalanceReply
		{
			Credit = result.Credit
		};
	}

	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName
		});
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var result = await fundInService.GetAsync(
			query: new FundInQuery
			{
				ApiUrl = new(request.ApiUrl),
				Deskey = request.Deskey,
				MD5key = request.MD5Key,
				AgentId = request.AgentId,
				AgentCurrency = request.AgentCurrency,
				PlayerCode = request.PlayerCode,
				OrderId = request.OrderId,
				Amount = request.Amount
			},
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply
		{
			Resp = result.Resp,
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var result = await fundOutService.GetAsync(
			query: new FundOutQuery
			{
				ApiUrl = new(request.ApiUrl),
				Deskey = request.Deskey,
				MD5key = request.MD5Key,
				AgentId = request.AgentId,
				AgentCurrency = request.AgentCurrency,
				PlayerCode = request.PlayerCode,
				OrderId = request.OrderId,
				Amount = request.Amount
			},
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply
		{
			Resp = result.Resp,
		};
	}

	public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
	{
		var state = await fundStatusService.GetAsync(
			query: new FundStatusQuery
			{
				ApiUrl = new(request.ApiUrl),
				Deskey = request.Deskey,
				MD5key = request.MD5Key,
				AgentId = request.AgentId,
				OrderId = request.OrderId
			},
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply
		{
			State = state
		};
	}

	public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
	{
		var results = await gameWagerService.QueryAsync(
			query: new GameRecordQuery
			{
				ApiUrl = new(request.ApiUrl),
				Deskey = request.Deskey,
				MD5key = request.MD5Key,
				AgentId = request.AgentId,
				StartTime = request.StartTime.ToDateTimeOffset(),
				EndTime = request.EndTime.ToDateTimeOffset()
			},
			cancellationToken: context.CancellationToken)
			.ToArrayAsync(
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameRecordResult>();

		var reply = new GameRecordReply();

		if (results.Any())
		{
			foreach (var result in results)
			{
				reply.Items.Add(new GameRecordInfo
				{
					PlayerCode = result.PlayerCode,
					ExternalBetId = result.ExternalBetId,
					ExternalGameId = result.ExternalGameId,
					ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
					ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
					Bet = result.Bet,
					ValidBet = result.ValidBet,
					Settle = result.Settle,
					Commission = result.Commission,
					WinLoss = result.WinLoss,
					AdditionalData = result.AdditionalData
				});
			}
		}

		return reply;
	}

	public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var result = await gameUrlService.GetAsync(
			query: new GameUrlQuery
			{
				ApiUrl = new(request.ApiUrl),
				Deskey = request.Deskey,
				MD5key = request.MD5Key,
				AgentId = request.AgentId,
				AgentCurrency = request.AgentCurrency,
				BrandCode = request.BrandCode,
				PlayerCode = request.PlayerCode,
				IpAddress = request.IpAddress,
				ExternalGameId = request.ExternalGameId
			},
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReply
		{
			GameUrl = result
		};
	}

	public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
	{
		var result = await playerBalanceService.GetAsync(
			query: new PlayerBalanceQuery
			{
				ApiUrl = new(request.ApiUrl),
				Deskey = request.Deskey,
				MD5key = request.MD5Key,
				AgentId = request.AgentId,
				AgentCurrency = request.AgentCurrency,
				PlayerCode = request.PlayerCode
			},
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceReply
		{
			Balance = result.Balance,
			FreeBalance = result.FreeBalance,
			OutstandingBalance = result.OutstandingBalance
		};
	}

	public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
	{
		await PlayerLogoutService.ExecuteAsync(
			command: new PlayerLogoutCommand
			{
				ApiUrl = new(request.ApiUrl),
				Deskey = request.Deskey,
				MD5key = request.MD5Key,
				AgentId = request.AgentId,
				PlayerCode = request.PlayerCode
			},
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply
		{
			State = true
		};
	}
}
