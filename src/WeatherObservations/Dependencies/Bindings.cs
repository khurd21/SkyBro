using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Ninject.Modules;
using WeatherObservations.Dependencies.DynamoDB;
using WeatherObservations.Dependencies.Http;
using WeatherObservations.Dependencies.Logger;
using WeatherObservations.Dependencies.WeatherObservations;

namespace WeatherObservations.Dependencies;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        // DynamoDB Injections
        Bind<AmazonDynamoDBConfig>().ToProvider<DynamoDBConfigProvider>();
        Bind<AmazonDynamoDBClient>().ToProvider<DynamoDBClientProvider>();
        Bind<DynamoDBContextConfig>().ToProvider<DynamoDBContextConfigProvider>();
        Bind<IDynamoDBContext>().ToProvider<DynamoDBContextProvider>();

        // Logger Injections
        Bind<ILogger>().To<WeatherObservationsLogger>();

        // Http Injections
        Bind<HttpClient>().ToProvider<HttpClientProvider>().InSingletonScope();

        // Weather Observations Injections
        Bind<ISkyConditionObservations>().To<SkyConditionObservations>();
    }
}