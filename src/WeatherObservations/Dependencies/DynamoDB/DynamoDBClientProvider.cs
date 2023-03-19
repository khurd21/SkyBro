using Amazon.DynamoDBv2;
using Ninject.Activation;

namespace WeatherObservations.Dependencies.DynamoDB;

public sealed class DynamoDBClientProvider : Provider<AmazonDynamoDBClient>
{
    private AmazonDynamoDBConfig Config { get; init; }

    public DynamoDBClientProvider(AmazonDynamoDBConfig config)
    {
        this.Config = config; 
    }

    protected override AmazonDynamoDBClient CreateInstance(IContext context)
    {
        return new(this.Config);
    }
}