using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record PlayerBalanceQuery
{
	[Required]
	[DefaultValue(typeof(Uri), "https://wc1-api.kewmn686.com/")]
	public Uri ApiUrl { get; set; } = default!;
	[Required]
	[DefaultValue(typeof(string), "706EB65DD6E6DF4C")]
	public string Deskey { get; set; } = default!;
	[Required]
	[DefaultValue(typeof(string), "3E00054B89B31B7A")]
	public string MD5key { get; set; } = default!;
	[Required]
	[DefaultValue(typeof(string), "200995")]
	public string AgentId { get; set; } = default!;
	[Required]
	[DefaultValue(typeof(string), "CNY")]
	public string AgentCurrency { get; set; } = default!;
	[Required]
	[DefaultValue(typeof(string), "4ucmqplxb029")]
	public string PlayerCode { get; set; } = default!;
}
