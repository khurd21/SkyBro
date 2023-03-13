using Amazon.DynamoDBv2;
using Ninject.Activation;

namespace WeatherObservations.Dependencies.DynamoDB;

public class DynamoDBConfigProvider : Provider<AmazonDynamoDBConfig>
{
    protected override AmazonDynamoDBConfig CreateInstance(IContext context)
    {
        throw new NotImplementedException();
    }
}