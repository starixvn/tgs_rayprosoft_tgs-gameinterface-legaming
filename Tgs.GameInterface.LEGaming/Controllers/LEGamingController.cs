using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
using Tgs.GameInterface.LEGaming.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.LEGaming.Controllers;

[Route("[controller]")]
[ApiController]
public class LEGamingController : ControllerBase
{
	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.kewmn686.com/",
	///             "deskey": "706EB65DD6E6DF4C",
	///             "mD5key": "3E00054B89B31B7A",
	///             "agentId": "200995",
	///             "playerCode":"4uk9399rb9qo",
	///             "orderId":"200995202307180247377734uk9399rb9qo",
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public ValueTask<string> FundInAsync(
		[FromServices] IGetService<FundInQuery, string> service,
		FundViewModel source)
	{
		var query = new FundInQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			PlayerCode: source.PlayerCode,
			OrderId: source.OrderId,
			Amount: source.Amount);

		return service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.kewmn686.com/",
	///             "deskey": "706EB65DD6E6DF4C",
	///             "mD5key": "3E00054B89B31B7A",
	///             "agentId": "200995",
	///             "playerCode":"4uk9399rb9qo",
	///             "orderId":"200995202307180240242304uk9399rb9qo",
	///             "amount": 1
	///         }
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public ValueTask<string> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, string> service,
		FundViewModel source)
	{
		var query = new FundOutQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			PlayerCode: source.PlayerCode,
			OrderId: source.OrderId,
			Amount: source.Amount);

		return service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.kewmn686.com/",
	///             "deskey": "706EB65DD6E6DF4C",
	///             "mD5key": "3E00054B89B31B7A",
	///             "agentId": "200995",
	///             "orderId":"200995202307180240242304uk9399rb9qo"
	///         }
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusViewModel source)
	{
		var query = new FundStatusQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			OrderId: source.OrderId);

		return service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Games the wager asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-record.kewmn686.com/",
	///             "deskey": "706EB65DD6E6DF4C",
	///             "mD5key": "3E00054B89B31B7A",
	///             "agentId": "200995",
	///             "startTime": "2023-07-18T11:59:00+08:00",
	///             "endTime": "2023-07-18T12:09:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("game/wager")]
	public async IAsyncEnumerable<GameWagerResultViewModel> GameWagerAsync(
		[FromServices] IQueryService<GameWagerQuery, GameWagerResult> service,
		GameWagerViewModel source)
	{
		var query = new GameWagerQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			StartTime: source.StartTime,
			EndTime: source.EndTime);

		var results = await service
			.QueryAsync(
				query: query,
				cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
				cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		if (!results.Any())
		{
			yield break;
		}

		foreach (var result in results)
		{
			yield return new GameWagerResultViewModel(
				ExternalWagerId: result.ExternalWagerId,
				ExternalGameId: result.ExternalGameId,
				PlayerCode: result.PlayerCode,
				ExternalCreatedOn: result.ExternalCreatedOn,
				ExternalClosedOn: result.ExternalClosedOn,
				Bet: result.Bet,
				ValidBet: result.ValidBet,
				Settle: result.Settle,
				Commission: result.Commission,
				WinLoss: result.WinLoss,
				AdditionalData: result.AdditionalData);
		}
	}

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.kewmn686.com/",
	///             "deskey": "706EB65DD6E6DF4C",
	///             "mD5key": "3E00054B89B31B7A",
	///             "agentId": "200995",
	///             "brandCode": "GPI_HKD",
	///             "playerCode":"4uk9399rb9qo",
	///             "ipAddress": "127.0.0.1",
	///             "gameExternalID": "0"
	///         }
	/// </remarks>
	[HttpPost("gameurl")]
	public ValueTask<string> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, string> service,
		GameUrlViewModel source)
	{
		var query = new GameUrlQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			BrandCode: source.BrandCode,
			PlayerCode: source.PlayerCode,
			IpAddress: source.IpAddress,
			ExternalGameId: source.ExternalGameId);

		return service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.kewmn686.com/",
	///             "deskey": "706EB65DD6E6DF4C",
	///             "mD5key": "3E00054B89B31B7A",
	///             "agentId": "200995",
	///             "playerCode":"4uk9399rb9qo"
	///         }
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		PlayerBalanceViewModel source)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			PlayerCode: source.PlayerCode);

		var result = await service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new PlayerBalanceResultViewModel(
			Balance: result.Balance,
			FreeBalance: result.FreeBalance,
			OutstandingBalance: result.OutstandingBalance);
	}

	/// <summary>
	/// Players the logout asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.kewmn686.com/",
	///             "deskey": "706EB65DD6E6DF4C",
	///             "mD5key": "3E00054B89B31B7A",
	///             "agentId": "200995",
	///             "playerCode":"4uk9399rb9qo"
	///         }
	/// </remarks>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutViewModel source)
	{
		var command = new PlayerLogoutCommand(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId,
			PlayerCode: source.PlayerCode);

		return service
			.ExecuteAsync(
			command: command,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Agents the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///         {
	///             "apiUrl": "https://wc1-api.kewmn686.com/",
	///             "deskey": "706EB65DD6E6DF4C",
	///             "mD5key": "3E00054B89B31B7A",
	///             "agentId": "200995"
	///         }
	/// </remarks>
	[HttpPost("agent/balance", Name = "AgentBalance")]
	public async ValueTask<AgentBalanceResultViewModel> AgentBalanceAsync(
		[FromServices] IGetService<AgentBalanceQuery, AgentBalanceResult> service,
		AgentBalanceViewModel source)
	{
		var query = new AgentBalanceQuery(
			ApiUrl: source.ApiUrl,
			Deskey: source.Deskey,
			MD5key: source.MD5key,
			AgentId: source.AgentId);

		var result = await service
			.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new AgentBalanceResultViewModel(
			Credit: result.Credit);
	}
}
