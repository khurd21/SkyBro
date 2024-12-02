using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace SkyBro.RequestHandler;

public class SessionEndedRequestHandler : ISkillRequestHandler
{
    public bool CanHandle(SkillRequest request) => request.Request is SessionEndedRequest;

    public SkillResponse Handle(SkillRequest request)
    {
        return ResponseBuilder.Tell("Blue skies!");
    }
}