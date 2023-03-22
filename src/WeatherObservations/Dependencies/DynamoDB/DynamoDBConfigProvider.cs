using Amazon.DynamoDBv2;
using Ninject.Activation;
using WeatherObservations.Data;

namespace WeatherObservations.Dependencies.DynamoDB;

public sealed class DynamoDBConfigProvider : Provider<AmazonDynamoDBConfig>
{
    protected override AmazonDynamoDBConfig CreateInstance(IContext context)
    {
        if (Configurations.IS_DEBUG)
        {
            return new AmazonDynamoDBConfig
            {
                ServiceURL = Configurations.DYNAMODB_LOCAL_SERVICE_URL,
            };
        }
        else
        {
            return new AmazonDynamoDBConfig()
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast2,
            };
        }
    }
}