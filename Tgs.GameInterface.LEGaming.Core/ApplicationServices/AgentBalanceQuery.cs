namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record AgentBalanceQuery(
	Uri ApiUrl,
	string Deskey,
	string MD5key,
	string AgentId);
