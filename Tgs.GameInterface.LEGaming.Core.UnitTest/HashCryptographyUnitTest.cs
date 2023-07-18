namespace Tgs.GameInterface.LEGaming.Core.UnitTest;

public class HashCryptographyUnitTest
{
    [Fact]
    public void Hash()
    {
        var hashCryptography = new HashCryptography();
        var agent = 200995;
        var timestamp = 1689570620501;
        var md5Key = "3E00054B89B31B7A";
        var data = $"{agent}{timestamp}{md5Key}";
        var actual = hashCryptography.GetHash(data: data);

        Assert.NotNull(actual);
        Assert.Equal("66b20ac5f09af38cc57496e1838a3ac3", actual);
    }

    [Fact]
    public async void AesEncrypt_Balance()
    {
        var hashCryptography = new HashCryptography();
        var secretKey = "706EB65DD6E6DF4C";
        var data = $"s={(int)SubTypes.Balance}&account=4uk9399rb9qo";
        var actual = await hashCryptography
            .GetAesEncryptAsync(
            data: data,
            secretKey: secretKey)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal("LxLJVtx15H2dgXaCSJNG9OQRmoSZ5fdqQNxK502gobo=", actual);
    }

    [Fact]
    public async void AesEncrypt_Deposit()
    {
        var hashCryptography = new HashCryptography();
        var secretKey = "706EB65DD6E6DF4C";
        var data = $"s={(int)SubTypes.Deposit}&account=4uk9399rb9qo&orderid=200995202307171346360524uk9399rb9qo&money=1";
        var actual = await hashCryptography
            .GetAesEncryptAsync(
            data: data,
            secretKey: secretKey)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal("9BKbWenbOpD7JD8jm2XOGBvbOmN+DvjUOwUBxG823tJfp1dhN6KO4zZMsa282vQOVNulFLKFd7kOfV9aycegC30o5d0qyzK3JSKSeG0Xe/8=", actual);
    }

    [Fact]
    public async void AesEncrypt_Withdrawal()
    {
        var hashCryptography = new HashCryptography();
        var secretKey = "706EB65DD6E6DF4C";
        var data = $"s={(int)SubTypes.Withdrawal}&account=4uk9399rb9qo&orderid=200995202307171354537634uk9399rb9qo&money=1";
        var actual = await hashCryptography
            .GetAesEncryptAsync(
            data: data,
            secretKey: secretKey)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal("GH0Qljygg7xF2c6Y96voiRvbOmN+DvjUOwUBxG823tJfp1dhN6KO4zZMsa282vQOhlWfJZ94ir3aDkBPGR+Vz30o5d0qyzK3JSKSeG0Xe/8=", actual);
    }

    [Fact]
    public async void AesEncrypt_Login()
    {
        var hashCryptography = new HashCryptography();
        var secretKey = "706EB65DD6E6DF4C";
        var data = $"s={(int)SubTypes.Login}&account=4uk9399rb9qo&money=0&orderid=200995202307171357417574uk9399rb9qo&ip=114.33.41.237&lineCode=GPI_HKD&KindID=0";
        var actual = await hashCryptography
            .GetAesEncryptAsync(
            data: data,
            secretKey: secretKey)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal("HQN9/G1xjatVFubjmWOt1qOD/F6kv2maQn09UTu/K8Wqcq16IBeEvLm3txrRJ8R8mSI2cnhHXmaAKiyWYjcvq50w9Kig2V9UKrPRprh4M+W5ernt8A5sSP0bq/WC/qJNKe8erVKIK0eFDUbnEgezppaGVQasxG/TJJBqRn0Chdc=", actual);
    }

    [Fact]
    public async void AesEncrypt_GameRecord()
    {
        var hashCryptography = new HashCryptography();
        var secretKey = "706EB65DD6E6DF4C";
        var data = $"s={(int)SubTypes.GameRecord}&startTime=1689573304660&endTime=1689573604660";
        var actual = await hashCryptography
            .GetAesEncryptAsync(
            data: data,
            secretKey: secretKey)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal("IvzEoNbMqRigzbXIyXr57C7DgbNK7cg3KxNUVy2ADR2MXV3MvhfBT75ritJ9mEBeQaUG2zsAqsctjCu/8MMYqg==", actual);
    }
}
