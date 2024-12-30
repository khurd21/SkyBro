using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace SkyBro.RequestHandler;

public class SkillRequestDispatcher : ISkillRequestDispatcher
{
    private IEnumerable<ISkillRequestHandler> Handlers { get; init; }

    public SkillRequestDispatcher(IEnumerable<ISkillRequestHandler> handlers)
    {
        Handlers = handlers;
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
        return ResponseBuilder.Tell("I'm sorry, I'm not sure how to help with that.");
    }
}