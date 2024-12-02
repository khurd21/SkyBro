using Alexa.NET.Request;
using Alexa.NET.Response;

namespace SkyBro.RequestHandler;

public interface ISkillRequestHandler
{
    bool CanHandle(SkillRequest request);
    SkillResponse Handle(SkillRequest request);
}