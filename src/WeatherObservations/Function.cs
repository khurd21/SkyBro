using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using WeatherObservations.Api;
using WeatherObservations.Data.DynamoDB;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace WeatherObservations;

public class Function
{
    private ILambdaLogger Logger { get; set; } = default!;

    public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
    {
        this.Logger = context.Logger;
        Type requestType = input.GetRequestType();
        context.Logger.LogLine($"Request Type: {requestType}");

        if (requestType == typeof(LaunchRequest)) {
            return this.HandleLaunchRequest();
        }
        else if (requestType == typeof(SessionEndedRequest)) {
            return this.HandleSessionEndRequest();
        }
        else if (requestType == typeof(IntentRequest)) {
            return this.HandleIntentRequest((input.Request as IntentRequest)!);
        }

        return this.HandleSessionEndRequest();
    }

    private SkillResponse HandleLaunchRequest() {
        string speech = new WeatherObservationsSpeechBuilder()
            .ReportIntroduction()
            .Speech;
        Reprompt rp = new("Ask me for sky conditions at Skydive Kapowsin.");
        return ResponseBuilder.Ask(speech, rp);
    }

    private SkillResponse HandleSessionEndRequest() => ResponseBuilder.Tell("Blue skies!");

    private SkillResponse HandleSessionHelpRequest()
    {
        const string speech = "You can ask me for sky conditions at an airport. " +
                                "For example, you can say, 'Alexa, ask Sky Bro for sky conditions at Skydive Kapowsin.' " +
                                "Or, you can say, 'Alexa, ask Sky Bro for sky conditions at Skydive Kapowsin for Saturday.' " +
                                "Please note, I can only provide sky conditions for up to three days in advance. ";

        Reprompt rp = new("Ask me for sky conditions at Skydive Kapowsin.");
        return ResponseBuilder.Ask(speech, rp);
    }

    private SkillResponse HandleGetSkyConditionsIntent(IntentRequest request)
    {
        string stationId = string.Empty;
        string state = string.Empty;
        DateTime date = default;
        try
        {
            string id = request.Intent.Slots[Function.AirportSlot].Resolution.Authorities[0].Values[0].Value.Id;
            stationId = id.Split(':')[0];
            state = id.Split(':')[1];

            string dateString = request.Intent.Slots[Function.DateSlot].SlotValue.Value;
            date = DateTime.Parse(dateString);
        }
        catch (Exception e)
        {
            this.Logger.LogError("Error parsing slot values.");
            this.Logger.LogError(e.Message);
        }

        if (string.IsNullOrEmpty(stationId) || string.IsNullOrEmpty(state))
        {
            return ResponseBuilder.Tell(new WeatherObservationsSpeechBuilder()
                .ReportNoObservationsForAirport()
                .Speech);
        }

        var skyConditions = AviationWeatherExtendedAPI.GetSkyConditionsExtended(stationId, state).Result;
        if (skyConditions == null || skyConditions.Count == 0)
        {
            return ResponseBuilder.Tell(
                new WeatherObservationsSpeechBuilder()
                    .ReportNoObservationsForAirport(stationId)
                    .Speech);
        }

        if (date == default)
        {
            date = skyConditions
                .Select(x => x.Value)
                .OrderByDescending(x => x.ObservationTimeUtc)
                .First()
                .ObservationTimeLocal;
        }

        var observations = skyConditions
            .Select(x => x.Value)
            .Where(x => x.ObservationTimeLocal.Date == date.Date)
            .ToList();

        if (observations.Count == 0)
        {
            return ResponseBuilder.Tell(
                new WeatherObservationsSpeechBuilder()
                    .ReportNoObservationsForAirport(stationId, date)
                    .Speech); 
        }

        return this.BuildSkyConditionsResponse(observations);
    }

    private SkillResponse BuildSkyConditionsResponse(IList<WeatherData> data)
    {
        return ResponseBuilder.Tell(
            new WeatherObservationsSpeechBuilder(data: data)
                .ReportIntroduction()
                .ReportDate()
                .ReportFlightRules()
                .ReportCloudConditions()
                .ReportWindConditions()
                .ReportPrecipitation()
                .ReportAverageTemperature()
                .Speech
        );
    }

    private SkillResponse HandleIntentRequest(IntentRequest request) {

        switch (request.Intent.Name) {
            case Function.CancelIntent:
            case Function.StopIntent:
                return this.HandleSessionEndRequest();
            case Function.HelpIntent:
                return this.HandleSessionHelpRequest();
            case Function.WeatherObservationsIntent:
                return this.HandleGetSkyConditionsIntent(request);
            default:
                return this.HandleSessionEndRequest();
        }
    }

    private const string AirportSlot = "airport";

    private const string DateSlot = "date";

    private const string CancelIntent = "AMAZON.CancelIntent";

    private const string StopIntent = "AMAZON.StopIntent";

    private const string HelpIntent = "AMAZON.HelpIntent";

    private const string WeatherObservationsIntent = "WeatherObservationsIntent";
}
