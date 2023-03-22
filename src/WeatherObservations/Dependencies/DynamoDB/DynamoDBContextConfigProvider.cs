using Amazon.DynamoDBv2.DataModel;
using Ninject.Activation;
using WeatherObservations.Data;

namespace WeatherObservations.Dependencies.DynamoDB;

public sealed class DynamoDBContextConfigProvider : Provider<DynamoDBContextConfig>
{
    protected override DynamoDBContextConfig CreateInstance(IContext context)
    {
        return new()
        {
            ConsistentRead = Configurations.DYNAMODB_CONSISTENT_READ,
            IgnoreNullValues = Configurations.DYNAMODB_IGNORE_NULL_VALUES,
            SkipVersionCheck = Configurations.DYNAMODB_SKIP_VERSION_CHECK,
            TableNamePrefix = Configurations.DYNAMODB_TABLE_NAME_PREFIX,
        };
    }
}