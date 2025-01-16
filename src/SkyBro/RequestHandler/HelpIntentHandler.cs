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
        return ResponseBuilder.Ask(
            @"<speak>
                Sky Bro provides weather observations for <say-as interpret-as=""characters"">USPA</say-as> affiliated drop zones across the United States.
                For example, ask me for weather at Skydive Kapowsin.
            </speak>",
            new Reprompt("Ask for weather at a drop zone of your choice.")
        );
    }
}