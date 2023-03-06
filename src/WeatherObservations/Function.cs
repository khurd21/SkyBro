using System.Net.NetworkInformation;
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
        string stationId = string.Empty;
        string state = string.Empty;
        DateTime date = default;
        try
        {
            string id = request.Intent.Slots["airport"].Resolution.Authorities[0].Values[0].Value.Id;
            stationId = id.Split(':')[0];
            state = id.Split(':')[1];

            string dateString = request.Intent.Slots["date"].SlotValue.Value;
            DateTime.TryParse(dateString, out date); 
        }
        catch (Exception)
        {
            // ignored
        }

        if (string.IsNullOrEmpty(stationId) || string.IsNullOrEmpty(state))
        {
            return ResponseBuilder.Tell("I'm sorry, I could not find any weather observations for that airport.");
        }
        if (date == default || date == null)
        {
            date = DateTime.Now;
        }

        var skyConditions = AviationWeatherExtendedAPI.GetSkyConditionsExtended(stationId, state).Result;
        if (skyConditions == null || skyConditions.Count == 0)
        {
            return ResponseBuilder.Tell("I'm sorry, I could not find any weather observations for that airport.");
        }

        // Get the observations that occur on the same day as the date provided. Only return the WeatherData object
        var observations = skyConditions
            .Select(x => x.Value)
            .Where(x => x.ObservationTime.GetValueOrDefault().Date == date.Date)
            .ToList();

        return this.BuildSkyConditionsResponse(observations);
    }

    private SkillResponse BuildSkyConditionsResponse(IList<WeatherData> data)
    {
        string speech = $"Weather observations for {data[0].StationID}. ";
        // Get average temperature
        var avgTemp = data.Average(x => x.TemperatureFahrenheit);
        speech += $"Average temperature: {Math.Round(avgTemp.GetValueOrDefault())} degrees Fahrenheit. ";
        speech += this.HandleSpeechForCloudBase(data);

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

    private string HandleSpeechForCloudBase(IList<WeatherData> data)
    {
        string speech = string.Empty;
        var orderedWeatherData = data
            .OrderByDescending(weatherData => weatherData.SkyConditions?
                .Max(skyConditions => skyConditions.CloudBaseFeetAGL));
        var maxCloudBaseObj = orderedWeatherData.FirstOrDefault();    
        var minCloudBaseObj = orderedWeatherData.LastOrDefault();

        if (maxCloudBaseObj == null || minCloudBaseObj == null)
        {
            return string.Empty;
        }

        var maxCloudBase = maxCloudBaseObj.SkyConditions?
            .OrderByDescending(skyConditions => skyConditions.CloudBaseFeetAGL)
            .FirstOrDefault();
        var minCloudBase = minCloudBaseObj.SkyConditions?
            .OrderByDescending(skyConditions => skyConditions.CloudBaseFeetAGL)
            .FirstOrDefault();

        if (maxCloudBase?.CloudBaseFeetAGL == minCloudBase?.CloudBaseFeetAGL)
        {
            speech += $"Cloud base {maxCloudBase?.CloudCoverPercent} percent at {maxCloudBase?.CloudBaseFeetAGL} feet. ";
        }
        else if (maxCloudBase?.CloudBaseFeetAGL != minCloudBase?.CloudBaseFeetAGL)
        {
            string upOrDown = maxCloudBaseObj?.ObservationTime > minCloudBaseObj?.ObservationTime ? "ascending" : "descending";
            var cloudBasesOrdered = new List<WeatherData?>() { maxCloudBaseObj, minCloudBaseObj }
                .OrderByDescending(wd => wd?.ObservationTime)
                .ToList();

            speech += $"Cloud base {cloudBasesOrdered[0]?.SkyConditions?[0].CloudBaseFeetAGL} feet at " +
            $"{cloudBasesOrdered[0]?.ObservationTime.GetValueOrDefault().Hour} {upOrDown} to " +
            $"{cloudBasesOrdered[1]?.SkyConditions?[0].CloudBaseFeetAGL} feet at " +
            $"{cloudBasesOrdered[1]?.ObservationTime.GetValueOrDefault().Hour}. ";
        }

        return speech;
    }
}
