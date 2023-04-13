using Amazon.DynamoDBv2;
using Ninject.Activation;
using WeatherObservations.Dependencies.DynamoDB;
using Xunit;

namespace WeatherObservations.Tests.Dependencies.DynamoDB;

internal class TestableDynamoDBConfigProvider : DynamoDBConfigProvider
{
    public TestableDynamoDBConfigProvider() : base()
    {
    }

    public new AmazonDynamoDBConfig CreateInstance(IContext context)
    {
        return base.CreateInstance(context);
    }
}

public class DynamoDBConfigProviderTests
{
    private TestableDynamoDBConfigProvider DynamoDBConfigProvider { get; set; }

    public DynamoDBConfigProviderTests()
    {
        this.DynamoDBConfigProvider = new();
    }

    [Fact]
    public void TestCreateInstance()
    {
        var config = this.DynamoDBConfigProvider.CreateInstance(null!);

        Assert.NotNull(config);
        Assert.IsType<AmazonDynamoDBConfig>(config);
    }
}