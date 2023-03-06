using System.Xml.Linq;

namespace WeatherObservations;

public static class AviationWeatherAPI
{
    static AviationWeatherAPI()
    {
        AviationWeatherAPI.Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows " +
        "NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
        "Chrome/86.0.4240.198 Edg/86.0.622.69");
    }
    public async static Task<IList<WeatherData>> GetSkyConditions(IList<string> stationIds)
    {
        stationIds = stationIds.Distinct().ToList();
        IList<WeatherData> skyConditions = new List<WeatherData>();
        IList<Task> skyConditionsTasks = new List<Task>();

        foreach (string stationId in stationIds)
        {
            skyConditionsTasks.Add(Task.Run(async () => {
                var skyCondition = await AviationWeatherAPI.GetSkyConditions(stationId);
                if (skyCondition != null) {
                    skyConditions.Add(skyCondition);
                }
            }));
        }
        await Task.WhenAll(skyConditionsTasks);
        return skyConditions;
    }

    public async static Task<WeatherData?> GetSkyConditions(string stationId)
    {

        var response = await AviationWeatherAPI.MakeWebRequest(new List<string>() { stationId });
        string responseContent = await response.Content.ReadAsStringAsync();
        XDocument xmlDocument = XDocument.Parse(responseContent);
        var item = xmlDocument.Descendants("METAR").FirstOrDefault();
        return item == null ? null : AviationWeatherAPI.XmlToWeatherData(item);
    }

    private async static Task<HttpResponseMessage> MakeWebRequest(IList<string> stationIds)
    {
        string requestUri = $"{WeatherObservationsGlobals.URL_FOR_WEATHER}{String.Join(',', stationIds)}";
        return await AviationWeatherAPI.Client.GetAsync(requestUri);
    }

    private static WeatherData XmlToWeatherData(XElement element)
    {
        return new()
        {
            RawText = element.Element(WeatherObservationsGlobals.RAW_TEXT)?.Value,
            StationID = element.Element(WeatherObservationsGlobals.STATION_ID)?.Value,
            FlightCategory = element.Element(WeatherObservationsGlobals.FLIGHT_CATEGORY)?.Value,

            ObservationTime = DateTime.Parse(
                element.Element(WeatherObservationsGlobals.OBSERVATION_TIME)?.Value ?? String.Empty,
                null,
                System.Globalization.DateTimeStyles.RoundtripKind),

            TemperatureCelsius = AviationWeatherAPI.TryParseFloat(element.Element(WeatherObservationsGlobals.TEMPERATURE_CELSIUS)?.Value),
            DewPointCelsius = AviationWeatherAPI.TryParseFloat(element.Element(WeatherObservationsGlobals.DEWPOINT_CELSIUS)?.Value),
            VisibilityStatuteMiles = AviationWeatherAPI.TryParseFloat(element.Element(WeatherObservationsGlobals.VISIBILITY_STATUTE_MILES)?.Value),
            AltimeterInHg = AviationWeatherAPI.TryParseFloat(element.Element(WeatherObservationsGlobals.ALTIMETER_IN_HG)?.Value),
            SeaLevelPressureMb = AviationWeatherAPI.TryParseFloat(element.Element(WeatherObservationsGlobals.SEA_LEVEL_PRESSURE_MB)?.Value),
            ElevationMeters = AviationWeatherAPI.TryParseFloat(element.Element(WeatherObservationsGlobals.ELEVATION_METERS)?.Value),
            PrecipitationInches = AviationWeatherAPI.TryParseFloat(element.Element(WeatherObservationsGlobals.PRECIPITATION_INCHES)?.Value),

            WindDirectionDegrees = AviationWeatherAPI.TryParseInt(element.Element(WeatherObservationsGlobals.WIND_DIRECTION_DEGREES)?.Value),
            WindSpeedKnots = AviationWeatherAPI.TryParseInt(element.Element(WeatherObservationsGlobals.WIND_SPEED_KNOTS)?.Value),
            WindGustKnots = AviationWeatherAPI.TryParseInt(element.Element(WeatherObservationsGlobals.WIND_GUST_KNOTS)?.Value),

            SkyConditions = new List<SkyConditions>()
            {
                new()
                {
                    CloudBaseFeetAGL = AviationWeatherAPI.TryParseInt(element
                        .Element(WeatherObservationsGlobals.SKY_CONDITIONS)?
                        .Attribute(WeatherObservationsGlobals.CLOUD_BASE_FEET_AGL)?.Value),
                    SkyCover = element
                        .Element(WeatherObservationsGlobals.SKY_CONDITIONS)?
                        .Attribute(WeatherObservationsGlobals.SKY_COVER)?.Value,
                },
            },
        };
    }

    private static float? TryParseFloat(string? input)
    {
        if (input == null) {
            return null;
        }

        float outValue;
        return float.TryParse(input, out outValue) ? (float?)outValue : null;
    }

    private static int? TryParseInt(string? input)
    {
        if (input == null) {
            return null;
        }

        int outValue;
        return int.TryParse(input, out outValue) ? (int?)outValue : null;
    }

    private static HttpClient Client { get; } = new();

}