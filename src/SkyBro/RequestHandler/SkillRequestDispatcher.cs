using System.Runtime.CompilerServices;

using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;

[assembly: InternalsVisibleTo("SkyBro.Tests")]

namespace SkyBro.RequestHandler;

public class SkillRequestDispatcher : ISkillRequestDispatcher
{

    internal IEnumerable<ISkillRequestHandler> Handlers { get; init; }

    public SkillRequestDispatcher()
    {
        Handlers = new List<ISkillRequestHandler>()
        {
            new LaunchRequestHandler(),
            new SessionEndedRequestHandler(),
            new IntentRequestHandler(),
        };
    }

    public SkillResponse Dispatch(SkillRequest request)
    {
        foreach (var handler in Handlers)
        {
            if (handler.CanHandle(request))
            {
                return handler.Handle(request);
            }
        }

        return ResponseBuilder.Tell("I'm sorry, I can't handle that request.");
    }
}