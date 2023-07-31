namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record PlayerBalanceResult
{
    public decimal Balance { get; init; }
    public decimal FreeBalance { get; init; }
    public decimal OutstandingBalance => Balance - FreeBalance;
}
