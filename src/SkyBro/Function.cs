using Alexa.NET.Request;
using Alexa.NET.Response;

using Amazon.Lambda.Core;

using SkyBro.RequestHandler;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SkyBro;

public class Function
{
    private ISkillRequestDispatcher SkillRequestDispatcher { get; init; }

    public Function()
    {
        SkillRequestDispatcher = DependencyResolver.Resolve<ISkillRequestDispatcher>();
    }

    /// <summary>
    /// Handles an Alexa Skill request and generates an appropriate response.
    /// </summary>
    /// <param name="request">The <see cref="SkillRequest"/> representing the incoming Alexa request.</param>
    /// <param name="context">The <see cref="ILambdaContext"/> providing methods for logging and information about the Lambda environment.</param>
    /// <returns>
    /// A <see cref="SkillResponse"/> containing the response to the Alexa request.
    /// </returns>
    public SkillResponse FunctionHandler(SkillRequest request, ILambdaContext context) => SkillRequestDispatcher.Dispatch(request);
}