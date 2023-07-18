using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.LEGaming.ViewModels;

public record FundStatusViewModel
{
    [Required]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string Deskey { get; init; } = default!;
    [Required]
    public string MD5key { get; init; } = default!;
    [Required]
    public string AgentId { get; init; } = default!;
    [Required]
    public string OrderId { get; init; } = default!;
}
