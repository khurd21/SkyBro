using Amazon.DynamoDBv2;
using Ninject.Activation;

namespace WeatherObservations.Dependencies.DynamoDB;

public class DynamoDBConfigProvider : Provider<AmazonDynamoDBConfig>
{
    protected override AmazonDynamoDBConfig CreateInstance(IContext context)
    {
        if (System.Diagnostics.Debugger.IsAttached)
        {
            return new AmazonDynamoDBConfig
            {
                ServiceURL = "http://localhost:8000"
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