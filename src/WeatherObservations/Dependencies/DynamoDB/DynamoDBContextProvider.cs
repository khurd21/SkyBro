using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Ninject.Activation;

namespace WeatherObservations.Dependencies.DynamoDB;

public class DynamoDBContextProvider : Provider<IDynamoDBContext>
{
    private AmazonDynamoDBClient Client { get; init; }

    private DynamoDBContextConfig Config { get; init; }

    public DynamoDBContextProvider(AmazonDynamoDBClient client, DynamoDBContextConfig config)
    {
        this.Client = client;
        this.Config = config;
    }

    protected override IDynamoDBContext CreateInstance(IContext context)
    {
        return new DynamoDBContext(this.Client, this.Config);
    }
}