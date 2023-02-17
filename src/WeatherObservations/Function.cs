using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

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
        string speech = "Aviation Weather Reporting";
        Reprompt rp = new("Ask me for weather at an airport.");
        return ResponseBuilder.Ask(speech, rp);
    }

    private SkillResponse HandleSessionEndRequest() => ResponseBuilder.Tell("Goodbye!");

    private SkillResponse HandleSessionHelpRequest()
    {
        const string speech = "You can ask me for sky conditions at an airport.";
        Reprompt rp = new("Ask me for sky conditions at an airport.");
        return ResponseBuilder.Ask(speech, rp);
    }

    private SkillResponse HandleGetSkyConditionsIntent(IntentRequest request)
    {
        string value = request.Intent.Slots["airport"].Value;
        string id = string.Empty;
        try
        {
            id = request.Intent.Slots["airport"].Resolution.Authorities[0].Values[0].Value.Id;
        }
        catch (Exception)
        {
            // ignored
        }

        // Add value to Task list to be run. If id is not empty, add it to the list to be run as well
        var weatherData = new List<WeatherData>();
        var tasks = new List<Task>();
        tasks.Add(Task.Run(async () => {
            var skyCondition = await AviationWeatherAPI.GetSkyConditions(value);
            if (skyCondition != null)
                weatherData.Add(skyCondition);
        }));
        if (!string.IsNullOrEmpty(id))
            tasks.Add(Task.Run(async () => {
                var skyCondition = await AviationWeatherAPI.GetSkyConditions(id);
                if (skyCondition != null)
                    weatherData.Add(skyCondition);
            }));

        // Wait for all tasks to complete
        Task.WaitAll(tasks.ToArray());

        if (weatherData.Count == 0)
            return ResponseBuilder.Tell($"I'm sorry, I could not find any weather observations for {value}.");
        if (weatherData.Count == 1)
            return this.BuildSkyConditionsResponse(weatherData[0]);
        if (weatherData.Count == 2)
        {
            // If the station IDs are the same, return the response
            if (weatherData[0].StationID == weatherData[1].StationID)
                return this.BuildSkyConditionsResponse(weatherData[0]);

            this.Logger.LogCritical($"WARNING: Two different station IDs were returned for the same airport. Value: {value}, ID: {id}, StationID1: {weatherData[0].StationID}, StationID2: {weatherData[1].StationID}");
            return this.BuildSkyConditionsResponse(weatherData[0]);
        }

        return ResponseBuilder.Tell($"I'm sorry, I could not find any weather observations for {value}.");
    }

    private SkillResponse BuildSkyConditionsResponse(WeatherData data)
    {
        string speech = $"Weather observations for {data.StationID}. ";
        // speech += $"Observation time: {data.ObservationTime}. ";
        if (data.FlightCategory != null)
            speech += $"Flight category: {data.FlightCategory}. ";
        speech += $"Wind: {data.WindDirectionDegrees} degrees at {data.WindSpeedKnots} knots";
        if (data.IsGusting) {
            speech += $" gusting to {data.WindGustKnots}";
        }
        speech += ". ";
        if (data.IsLightning)
        {
            speech += "Warning. Lightning is present. ";
        }

        if (data.SkyConditions != null)
            foreach (var skyCondition in data.SkyConditions) {
                speech += $"Sky cover: {skyCondition.SkyCover}";
                if (skyCondition.CloudBaseFeetAGL != null) {
                    speech += $" with cloud base at {skyCondition.CloudBaseFeetAGL} feet";
                }
                speech += ". ";
            }
        
        if (data.VisibilityStatuteMiles != null)
            speech += $"Visibility: {data.VisibilityStatuteMiles} miles. ";
        if (data.TemperatureCelsius != null)
            speech += $"Temperature: {data.TemperatureCelsius} degrees Celsius. ";
        if (data.DewPointCelsius != null)
            speech += $"Dew point: {data.DewPointCelsius} degrees Celsius. ";
        if (data.AltimeterInHg != null)
            speech += $"Altimeter: {data.AltimeterInHg}. ";
        if (data.SeaLevelPressureMb != null)
            speech += $"Sea level pressure: {data.SeaLevelPressureMb}. ";
        if (data.ElevationMeters != null)
            speech += $"Elevation: {data.ElevationMeters} meters. ";

        return ResponseBuilder.Tell(speech);
    }

    private SkillResponse HandleIntentRequest(IntentRequest request) {

        switch (request.Intent.Name) {
            case "AMAZON.CancelIntent":
            case "AMAZON.StopIntent":
                return this.HandleSessionEndRequest();
            case "AMAZON.HelpIntent":
                return this.HandleSessionHelpRequest();
            case "WeatherObservationsIntent":
                return this.HandleGetSkyConditionsIntent(request);
            default:
                return this.HandleSessionEndRequest();
        }

    }
}
