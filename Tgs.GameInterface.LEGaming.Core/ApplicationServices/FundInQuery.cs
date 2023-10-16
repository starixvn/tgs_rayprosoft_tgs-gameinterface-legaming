namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string Deskey,
    string MD5key,
    string AgentId,
    string PlayerCode,
    string OrderId,
    decimal Amount);
