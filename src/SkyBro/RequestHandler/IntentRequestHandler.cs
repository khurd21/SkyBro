using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace SkyBro.RequestHandler;

public class IntentRequestHandler : ISkillRequestHandler
{
    public bool CanHandle(SkillRequest request) => request.Request is IntentRequest;

    public SkillResponse Handle(SkillRequest request)
    {
        var intentRequest = (IntentRequest)request.Request;
        var intentHandlers = new Dictionary<string, Func<SkillResponse>>()
        {
            { "AMAZON.HelpIntent", () => ResponseBuilder.Ask("Here to help! Ask me anything.", new Reprompt("Ask for weather at Skydive Kapowsin.")) },
            { "AMAZON.CancelIntent", () => ResponseBuilder.Tell("Blue skies!") },
            { "WeatherObservationsIntent", () => ResponseBuilder.Tell("Weather observations.") },
        };

        if (intentHandlers.TryGetValue(intentRequest.Intent.Name, out var handler))
        {
            return handler();
        }
        return ResponseBuilder.Tell("I'm not sure how to help with that.");
    }
}