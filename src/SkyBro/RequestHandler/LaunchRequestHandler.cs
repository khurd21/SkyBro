using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace SkyBro.RequestHandler;

public class LaunchRequestHandler : ISkillRequestHandler
{
    public bool CanHandle(SkillRequest request) => request.Request is LaunchRequest;

    public SkillResponse Handle(SkillRequest request)
    {
        var reprompt = new Reprompt("Ask me for the weather at a dropzone. For example, say 'what is the weather at Skydive Kapowsin?'.");
        return ResponseBuilder.Ask("Welcome to SkyBro! To start, ask me for the weather at a dropzone of your choice.", reprompt);
    }
}