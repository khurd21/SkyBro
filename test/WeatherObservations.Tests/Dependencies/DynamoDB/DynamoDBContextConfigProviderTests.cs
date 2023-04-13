using Amazon.DynamoDBv2.DataModel;
using Ninject.Activation;
using WeatherObservations.Dependencies.DynamoDB;
using Xunit;

namespace WeatherObservations.Tests.Dependencies.DynamoDB;

internal class TestableDynamoDBContextConfigProvider : DynamoDBContextConfigProvider
{
    public TestableDynamoDBContextConfigProvider() : base()
    {
    }

    public new DynamoDBContextConfig CreateInstance(IContext context)
    {
        return base.CreateInstance(context);
    }
}

public class DynamoDBContextConfigProviderTests
{
    private TestableDynamoDBContextConfigProvider DynamoDBContextConfigProvider { get; set; }

    public DynamoDBContextConfigProviderTests()
    {
        this.DynamoDBContextConfigProvider = new();
    }

    [Fact]
    public void TestCreateInstance()
    {
        var config = this.DynamoDBContextConfigProvider.CreateInstance(null!);

        Assert.NotNull(config);
        Assert.IsType<DynamoDBContextConfig>(config);
    }
}