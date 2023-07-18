namespace Tgs.GameInterface.LEGaming.Core;

public interface IHashCryptography
{
    ValueTask<string> GetAesEncryptAsync(
        string data,
        string secretKey,
        CancellationToken cancellationToken = default);

    string GetHash(string data);
}
