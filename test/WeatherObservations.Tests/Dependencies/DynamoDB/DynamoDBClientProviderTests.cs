using Amazon.DynamoDBv2;
using Moq;
using Ninject.Activation;
using WeatherObservations.Dependencies.DynamoDB;
using Xunit;

namespace WeatherObservations.Tests.Dependencies.DynamoDB;

internal class TestableDynamoDBClientProvider : DynamoDBClientProvider
{
    public TestableDynamoDBClientProvider(AmazonDynamoDBConfig config) : base(config)
    {
    }

    public new AmazonDynamoDBClient CreateInstance(IContext context)
    {
        return base.CreateInstance(context);
    }
}

public class DynamoDBClientProviderTests
{
    private Mock<AmazonDynamoDBConfig> Config { get; set; }

    private TestableDynamoDBClientProvider DynamoDBClientProvider { get; set; }

    public DynamoDBClientProviderTests()
    {
        this.Config = new();
        this.DynamoDBClientProvider = new(this.Config.Object);
    }

    [Fact]
    public void TestCreateInstance()
    {
        var client = this.DynamoDBClientProvider.CreateInstance(null!);

        Assert.NotNull(client);
        Assert.IsType<AmazonDynamoDBClient>(client); 
    }

}