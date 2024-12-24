using Alexa.NET.Request;
using Alexa.NET.Response;

namespace SkyBro.RequestHandler;

public interface ISkillRequestDispatcher
{
    public SkillResponse Dispatch(SkillRequest request);
}