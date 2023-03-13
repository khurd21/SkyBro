using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Ninject.Modules;
using WeatherObservations.Dependencies.DynamoDB;

namespace WeatherObservations.Dependencies;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<AmazonDynamoDBConfig>().ToProvider<DynamoDBConfigProvider>();
        Bind<IDynamoDBContext>().ToMethod(context => new DynamoDBContext(new AmazonDynamoDBClient()));
        throw new NotImplementedException();
    }

    private AmazonDynamoDBClient CreateClient()
    {
        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:8000"
        };
        return new AmazonDynamoDBClient(config);
    }
}