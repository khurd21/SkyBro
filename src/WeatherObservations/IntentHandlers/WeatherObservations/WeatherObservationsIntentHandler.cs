using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using WeatherObservations.Data.DynamoDB;
using WeatherObservations.Dependencies.Logger;
using WeatherObservations.Dependencies.WeatherObservations;
using WeatherObservations.SpeechBuilder;

namespace WeatherObservations.IntentHandlers.WeatherObservations;

public class WeatherObservationsIntentHandler : IWeatherObservationsIntentHandler
{
    public string IntentName { get; } = IntentFactory.WeatherObservationsIntent;

    private ILogger Logger { get; init; }

    private ISkyConditionObservations SkyConditionObservations { get; init; }

    private const string AirportSlot = "airport";

    private const string DateSlot = "date";

    public WeatherObservationsIntentHandler(ILogger logger, ISkyConditionObservations observations)
    {
        this.Logger = logger;
        this.SkyConditionObservations = observations;
    }

    public async Task<SkillResponse> HandleIntentAsync(IntentRequest request)
    {
        this.Logger.Log($"Handling intent: {request.Intent.Name}.");
        string stationId = string.Empty;
        string state = string.Empty;
        DateTime date = default;
        try
        {
            string id = request.Intent.Slots[AirportSlot].Resolution.Authorities[0].Values[0].Value.Id;
            stationId = id.Split(':')[0];
            state = id.Split(':')[1];

            string dateString = request.Intent.Slots[DateSlot].SlotValue.Value;
            date = DateTime.Parse(dateString);
        }
        catch (Exception e)
        {
            this.Logger.Log("Error parsing slot values.");
            this.Logger.Log(e.Message);
        }

        if (string.IsNullOrEmpty(stationId) || string.IsNullOrEmpty(state))
        {
            return ResponseBuilder.Tell(new WeatherObservationsSpeechBuilder()
                .ReportNoObservationsForAirport()
                .Build());
        }

        var skyConditions = await this.SkyConditionObservations.GetSkyConditionsAsync(stationId, state);
        if (skyConditions == null || skyConditions.Count == 0)
        {
            return ResponseBuilder.Tell(
                new WeatherObservationsSpeechBuilder()
                    .ReportNoObservationsForAirport(stationId)
                    .Build());
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
                    .Build()); 
        }

        return this.BuildSkyConditionsResponse(observations);
    }

    private SkillResponse BuildSkyConditionsResponse(IList<WeatherData> observations)
    {
        return ResponseBuilder.Tell(
            new WeatherObservationsSpeechBuilder(data: observations)
                .ReportIntroduction()
                .ReportDate()
                .ReportFlightRules()
                .ReportCloudConditions()
                .ReportWindConditions()
                .ReportPrecipitation()
                .ReportAverageTemperature()
                .Build()
        );
    }
}