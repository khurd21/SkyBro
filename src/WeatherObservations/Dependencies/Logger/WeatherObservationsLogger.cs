using Amazon.Lambda.Core;

namespace WeatherObservations.Dependencies.Logger;

public class WeatherObservationsLogger : ILogger
{
    public void Log(string message)
    {
        LambdaLogger.Log(message);
    }
}