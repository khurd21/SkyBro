using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Moq;
using Ninject.Activation;
using WeatherObservations.Dependencies.DynamoDB;
using Xunit;

namespace WeatherObservations.Tests.Dependencies.DynamoDB;

internal class TestableDynamoDBContextProvider : DynamoDBContextProvider
{
    public TestableDynamoDBContextProvider(
        AmazonDynamoDBClient client,
        DynamoDBContextConfig config) : base(client, config)
    {
    }

    public new IDynamoDBContext CreateInstance(IContext context)
    {
        return base.CreateInstance(context);
    }
}

public class DynamoDBContextProviderTests
{
    private TestableDynamoDBContextProvider DynamoDBContextProvider { get; set; }

    private Mock<AmazonDynamoDBClient> Client { get; set; }

    private Mock<DynamoDBContextConfig> Config { get; set; }

    public DynamoDBContextProviderTests()
    {
        this.Client = new();
        this.Config = new();
        this.DynamoDBContextProvider = new(this.Client.Object, this.Config.Object);
    }

    [Fact]
    public void TestCreateInstance()
    {
        var context = this.DynamoDBContextProvider.CreateInstance(null!);

        Assert.NotNull(context);
        Assert.IsType<DynamoDBContext>(context);
    }
}