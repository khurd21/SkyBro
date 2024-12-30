using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace SkyBro.RequestHandler;

public class CancelIntentHandler : ISkillRequestHandler
{
    private static string Name => "AMAZON.CancelIntent";

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
        return ResponseBuilder.Tell(new SsmlOutputSpeech
        {
            Ssml =
            @"<speak>
                <amazon:emotion name='excited' intensity='low'>
                    Blue skies!
                </amazon:emotion>
            </speak>"
        });
    }

}