using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
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

public class AmazonDynamoDBClientWrapper : AmazonDynamoDBClient
{
    public virtual IClientConfig ConfigWrapper => base.Config;
}

public class DynamoDBContextProviderTests
{
    private TestableDynamoDBContextProvider DynamoDBContextProvider { get; set; }

    private Mock<AmazonDynamoDBClientWrapper> Client { get; set; }

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
        // Error: No RegionEndpoint or ServiceURL configured
        this.Client.SetupGet(x => x.ConfigWrapper.RegionEndpoint)
            .Returns(RegionEndpoint.USEast1)
            .Verifiable();
        
        this.Client.SetupGet(x => x.ConfigWrapper.ServiceURL)
            .Returns("http://localhost:8000")
            .Verifiable();

        var context = this.DynamoDBContextProvider.CreateInstance(null!);

        Assert.NotNull(context);
        Assert.IsType<DynamoDBContext>(context);
        Mock.Verify();
    }
}