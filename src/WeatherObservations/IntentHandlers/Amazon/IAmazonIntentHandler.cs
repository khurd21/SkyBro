using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace WeatherObservations.IntentHandlers.Amazon;

public interface IAmazonIntentHandler : IIntentHandler
{
    Task<SkillResponse> HandleCancelIntent(IntentRequest request);

    Task<SkillResponse> HandleHelpIntent(IntentRequest request);

    Task<SkillResponse> HandleStopIntent(IntentRequest request);
}