using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace WeatherObservations.IntentHandlers.Amazon;

public interface IAmazonIntentHandler : IIntentHandler
{
    Task<SkillResponse> HandleCancelIntentAsync(IntentRequest request);

    Task<SkillResponse> HandleHelpIntentAsync(IntentRequest request);

    Task<SkillResponse> HandleStopIntentAsync(IntentRequest request);
}