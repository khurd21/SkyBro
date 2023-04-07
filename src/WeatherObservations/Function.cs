using System.Reflection;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Ninject;
using WeatherObservations.Dependencies.Logger;
using WeatherObservations.Dependencies.WeatherObservations;
using WeatherObservations.IntentHandlers;
using WeatherObservations.SpeechBuilder;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace WeatherObservations;

public class Function
{
    private ILogger Logger { get; init; }

    private ISkyConditionObservations SkyConditionObservations { get; init; }

    public Function()
    {
        var kernel = new StandardKernel();
        kernel.Load(Assembly.GetExecutingAssembly());

        // Get Injections
        this.Logger = kernel.Get<ILogger>();
        this.SkyConditionObservations = kernel.Get<ISkyConditionObservations>();
    }

    public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
    {
        Type requestType = input.GetRequestType();
        this.Logger.Log($"Request Type: {requestType}");

        if (requestType == typeof(LaunchRequest))
        {
            return this.HandleLaunchRequest();
        }
        else if (requestType == typeof(SessionEndedRequest))
        {
            var request = input.Request as SessionEndedRequest;
            return this.HandleSessionEndRequest();
        }
        else if (requestType == typeof(IntentRequest))
        {
            var request = (IntentRequest)input.Request;
            IIntentHandler intentHandler = IntentFactory.GetIntentHandler(request.Intent.Name);
            return intentHandler.HandleIntent(request).Result;
        }

        return this.HandleSessionEndRequest();
    }

    private SkillResponse HandleLaunchRequest() {
        string speech = new WeatherObservationsSpeechBuilder()
            .ReportIntroduction()
            .Build();
        Reprompt rp = new("Ask me for sky conditions at Skydive Kapowsin.");
        return ResponseBuilder.Ask(speech, rp);
    }

    private SkillResponse HandleSessionEndRequest() => ResponseBuilder.Tell("Blue skies!");
}
