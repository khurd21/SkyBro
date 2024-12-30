using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace SkyBro.RequestHandler;

public class HelpIntentHandler : ISkillRequestHandler
{
    private static string Name => "AMAZON.HelpIntent";

    public bool CanHandle(SkillRequest request)
    {
        if (request.Request is IntentRequest intentRequest)
        {
            return Name.Equals(intentRequest.Intent.Name);
        }
        return false;
    }

    public SkillResponse Handle(SkillRequest request)
    {
        return ResponseBuilder.Ask("Here to help! Ask me anything.", new Reprompt("Ask for weather at Skydive Kapowsin."));
    }
}