using System.Reflection;
using Ninject;
using WeatherObservations.IntentHandlers.Amazon;
using WeatherObservations.IntentHandlers.WeatherObservations;

namespace WeatherObservations.IntentHandlers;

public static class IntentFactory
{
    public const string WeatherObservationsIntent = "WeatherObservationsIntent";

    public const string AmazonIntent = "AMAZON";

    private static readonly Dictionary<string, Type> IntentHandlers = new()
    {
        { WeatherObservationsIntent, typeof(IWeatherObservationsIntentHandler) },
        { AmazonIntent, typeof(IAmazonIntentHandler) }
    };

    public static IIntentHandler GetIntentHandler(string intentName)
    {
        if (!IntentHandlers.ContainsKey(intentName))
        {
            throw new KeyNotFoundException($"Intent {intentName} not found");
        }

        var kernel = new StandardKernel();
        kernel.Load(Assembly.GetExecutingAssembly());
        return (IIntentHandler)kernel.Get(IntentHandlers[intentName]);
    }
}