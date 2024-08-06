namespace Tgs.GameInterface.LEGaming.Core.Models;

public record BalanceResp(
	bool IsPlayerExist,
	decimal TotalMoney,
	decimal FreeMoney);
