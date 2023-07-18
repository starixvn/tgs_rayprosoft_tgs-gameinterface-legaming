namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,
    string Deskey,
    string MD5key,
    string AgentId,
    string OrderId);
