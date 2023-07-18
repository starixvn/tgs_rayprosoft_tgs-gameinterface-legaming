namespace Tgs.GameInterface.LEGaming.Core.ApplicationServices;

public record GameRecordQuery(
    Uri ApiUrl,
    string Deskey,
    string MD5key,
    string AgentId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
