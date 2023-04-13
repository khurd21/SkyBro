using Ninject;
using WeatherObservations.IntentHandlers.Amazon;
using WeatherObservations.IntentHandlers.WeatherObservations;

namespace WeatherObservations.IntentHandlers;

public class IntentFactory
{
    public const string WeatherObservationsIntent = "WeatherObservationsIntent";

    public const string AmazonIntent = "AMAZON";

    private static readonly Dictionary<string, Type> IntentHandlers = new()
    {
        { WeatherObservationsIntent, typeof(IWeatherObservationsIntentHandler) },
        { AmazonIntent, typeof(IAmazonIntentHandler) }
    };

    private IKernel Kernel { get; init; }

    public IntentFactory(IKernel kernel)
    {
        this.Kernel = kernel;
    }

    public IIntentHandler GetIntentHandler(string intentName)
    {
        if (!IntentHandlers.ContainsKey(intentName))
        {
            throw new KeyNotFoundException($"Intent {intentName} not found");
        }

        return (IIntentHandler)this.Kernel.Get(IntentHandlers[intentName]);
    }
}