using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using WeatherObservations.Dependencies.Logger;

namespace WeatherObservations.IntentHandlers.Amazon;

public class AmazonIntentHandler : IAmazonIntentHandler
{
    public string IntentName { get; } = IntentFactory.AmazonIntent;

    private ILogger Logger { get; init; }

    private const string CancelIntent = "CancelIntent";

    private const string HelpIntent = "HelpIntent";

    private const string StopIntent = "StopIntent";

    public AmazonIntentHandler(ILogger logger)
    {
        this.Logger = logger;
    }

    public async Task<SkillResponse> HandleIntent(IntentRequest request)
    {
        this.Logger.Log($"Handling intent: {request.Intent.Name}.");
        // parse the intent name: "AMAZON.CancelIntent"
        string intentNameOrigin = request.Intent.Name.Split('.').First();
        string intentName = request.Intent.Name.Split('.').Last();

        switch (intentName) {
            case CancelIntent:
                return await this.HandleCancelIntentAsync(request);
            case HelpIntent:
                return await this.HandleHelpIntentAsync(request);
            case StopIntent:
                return await this.HandleStopIntentAsync(request);
            default:
                this.Logger.Log($"Unhandled intent: {intentNameOrigin}");
                return await this.HandleHelpIntentAsync(request);
        }
    }

    public async Task<SkillResponse> HandleCancelIntentAsync(IntentRequest request)
    {
        return await this.HandleStopIntentAsync(request);
    }

    public async Task<SkillResponse> HandleHelpIntentAsync(IntentRequest request)
    {
        const string speech = "You can ask me for sky conditions at an airport. " +
                                "For example, you can say, 'Alexa, ask Sky Bro for sky conditions at Skydive Kapowsin.' " +
                                "Or, you can say, 'Alexa, ask Sky Bro for sky conditions at Skydive Kapowsin for Saturday.' " +
                                "Please note, I can only provide sky conditions for up to three days in advance. ";

        Reprompt rp = new("Ask me for sky conditions at Skydive Kapowsin.");
        return await Task.FromResult(ResponseBuilder.Ask(speech, rp));
    }

    public async Task<SkillResponse> HandleStopIntentAsync(IntentRequest request)
    {
        return await Task.FromResult(ResponseBuilder.Tell("Blue skies!"));
    }
}