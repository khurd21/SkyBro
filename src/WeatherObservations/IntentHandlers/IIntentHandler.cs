using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace WeatherObservations.IntentHandlers;

public interface IIntentHandler
{
    string IntentName { get; }

    Task<SkillResponse> HandleIntent(IntentRequest request);
}