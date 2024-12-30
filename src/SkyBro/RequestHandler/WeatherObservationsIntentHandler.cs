using System.Text;

using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

using Ninject;

using SkyBro.Metar;
using SkyBro.Metar.Models;
using SkyBro.Services;

namespace SkyBro.RequestHandler;

public class WeatherObservationsIntentHandler : ISkillRequestHandler
{
    private IMetarEndpoint WeatherClient { get; init; }

    private static string Name => "WeatherObservationsIntent";

    [Inject]
    public WeatherObservationsIntentHandler(IMetarEndpoint weatherClient)
    {
        WeatherClient = weatherClient;
    }

    public bool CanHandle(SkillRequest request)
    {
        if (request.Request is IntentRequest intentRequest)
        {
            return Name.Equals(intentRequest.Intent.Name);
        }
        return false;
    }


    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public SkillResponse Handle(SkillRequest request)
    {
        var intentRequest = (IntentRequest)request.Request;
        var stationIdSlot = intentRequest.Intent.Slots["StationId"];
        if (stationIdSlot?.Resolution.Authorities.First().Status.Code.Equals("ER_SUCCESS_NO_MATCH") ?? true)
        {
            return ResponseBuilder.Tell("I'm sorry, I couldn't process your request. Please try again.");
        }

        var stationValue = stationIdSlot?.Resolution.Authorities.First().Values.First().Value!;
        var parts = stationValue.Id.Split(";");
        if (parts.Length != 3)
        {
            return ResponseBuilder.Tell("I'm sorry, I couldn't process your request. Please try again.");
        }

        var stationCode = parts[0];
        var latitude = parts[1];
        var longitude = parts[2];

        APIResponse<MetarStationData>? result = null;
        if (!string.IsNullOrEmpty(stationCode))
        {
            result = WeatherClient.GetNearestMetarAsync(stationCode).Result;
        }
        else if (decimal.TryParse(latitude, out decimal lat) && decimal.TryParse(longitude, out decimal lon))
        {
            var geolocationCoordinate = new Metar.Models.GeolocationCoordinate
            {
                Latitude = lat,
                Longitude = lon
            };
            result = WeatherClient.GetNearestMetarAsync(geolocationCoordinate).Result;
        }

        if (result?.Data == null || !result.Success)
        {
            return ResponseBuilder.Tell($"I am sorry. I could not find weather observations for {stationValue.Name}");
        }

        var stationData = result.Data;
        var responseText = new StringBuilder($"Weather observations for {stationValue.Name}. ");

        if (stationData.Wind != null)
        {
            responseText.Append($"Wind: {stationData.Wind.SpeedMph} miles per hour");
            if (stationData.Wind.GustMph != null)
            {
                responseText.Append($" with gusts up to {stationData.Wind.GustMph}");
            }
            responseText.Append(". ");
        }

        if (!string.IsNullOrEmpty(stationData.FlightCategory))
        {
            responseText.Append($"Flight category: {string.Join(".", stationData.FlightCategory.ToCharArray())}. ");
        }

        if (stationData.Visibility != null)
        {
            responseText.Append($"Visibility: {stationData.Visibility.Miles} miles. ");
        }

        if (stationData.Rain != null)
        {
            responseText.Append($"Rain: {stationData.Rain.Inches} inches. ");
        }

        if (stationData.Snow != null)
        {
            responseText.Append($"Snow: {stationData.Snow.Inches} inches. ");
        }

        if (stationData.Clouds != null && stationData.Clouds.Any())
        {
            responseText.Append("Cloud cover: ");
            foreach (var cloud in stationData.Clouds)
            {
                responseText.Append($"{cloud.Text}");
                if (cloud.Feet != null)
                {
                    responseText.Append($" at {cloud.Feet} feet. ");
                }
            }
        }

        if (stationData.Temperature != null)
        {
            responseText.Append($"Temperature: {stationData.Temperature.Fahrenheit} degrees fahrenheit. ");
        }

        if (stationData.Dewpoint != null)
        {
            responseText.Append($"Dewpoint: {stationData.Dewpoint.Fahrenheit} degrees fahrenheit.");
        }

        return ResponseBuilder.Tell(responseText.ToString());
    }
}