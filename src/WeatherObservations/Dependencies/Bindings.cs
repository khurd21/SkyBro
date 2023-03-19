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
        Bind<AmazonDynamoDBClient>().ToProvider<DynamoDBClientProvider>();
        Bind<DynamoDBContextConfig>().ToProvider<DynamoDBContextConfigProvider>();
        Bind<IDynamoDBContext>().ToProvider<DynamoDBContextProvider>();
    }
}