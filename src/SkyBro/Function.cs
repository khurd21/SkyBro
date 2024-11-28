using Alexa.NET.Request;
using Alexa.NET.Response;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SkyBro;

public class Function
{
    /// <summary>
    /// Handles an Alexa Skill request and generates an appropriate response.
    /// </summary>
    /// <param name="request">The <see cref="SkillRequest"/> representing the incoming Alexa request.</param>
    /// <param name="context">The <see cref="ILambdaContext"/> providing methods for logging and information about the Lambda environment.</param>
    /// <returns>
    /// A <see cref="SkillResponse"/> containing the response to the Alexa request.
    /// </returns>
    public SkillResponse FunctionHandler(SkillRequest request, ILambdaContext context)
    {
        return new();
    }
}