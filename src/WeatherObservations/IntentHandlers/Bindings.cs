using Amazon.Lambda.Core;
using Ninject.Modules;
using WeatherObservations.IntentHandlers.Amazon;
using WeatherObservations.IntentHandlers.WeatherObservations;

namespace WeatherObservations.IntentHandlers;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<IWeatherObservationsIntentHandler>().To<WeatherObservationsIntentHandler>();
        Bind<IAmazonIntentHandler>().To<AmazonIntentHandler>();

        Bind<IIntentHandler>()
            .To<AmazonIntentHandler>()
            .When(request => request.Target.Name == IntentFactory.AmazonIntent);
        Bind<IIntentHandler>()
            .To<WeatherObservationsIntentHandler>()
            .When(request => request.Target.Name == IntentFactory.WeatherObservationsIntent);
    }
}