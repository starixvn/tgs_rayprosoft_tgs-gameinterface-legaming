namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record GameWagerQuery(
	Uri ApiUrl,
	string Deskey,
	string MD5key,
	string AgentId,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);
