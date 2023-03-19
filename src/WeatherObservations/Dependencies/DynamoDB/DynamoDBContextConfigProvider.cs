using Amazon.DynamoDBv2.DataModel;
using Ninject.Activation;

namespace WeatherObservations.Dependencies.DynamoDB;

public sealed class DynamoDBContextConfigProvider : Provider<DynamoDBContextConfig>
{
    protected override DynamoDBContextConfig CreateInstance(IContext context)
    {
        return new()
        {
            ConsistentRead = true,
            IgnoreNullValues = true,
            SkipVersionCheck = true,
            TableNamePrefix = null,
        };
    }
}