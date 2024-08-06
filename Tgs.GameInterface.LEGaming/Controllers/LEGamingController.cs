using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.LEGaming.Core.ApplicationServices;
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
	[HttpPost("fund/in", Name = "FundIn")]
	public ValueTask<FundResult> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		FundInQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("fund/out", Name = "FundOut")]
	public ValueTask<FundResult> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		FundOutQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the wager asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("game/gecord")]
	public IAsyncEnumerable<GameRecordResult> GameWagerAsync(
		[FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
		GameRecordQuery source)
		=> service.QueryAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

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
		GameUrlQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public ValueTask<PlayerBalanceResult> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		PlayerBalanceQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the logout asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutCommand source)
		=> service.ExecuteAsync(
			command: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Agents the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("agent/balance", Name = "AgentBalance")]
	public ValueTask<AgentBalanceResult> AgentBalanceAsync(
		[FromServices] IGetService<AgentBalanceQuery, AgentBalanceResult> service,
		AgentBalanceQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);
}
