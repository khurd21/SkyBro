using System.Globalization;
using HtmlAgilityPack;
using WeatherObservations.Data;

namespace WeatherObservations.Api;

public static class AviationWeatherExtendedAPI
{
    static AviationWeatherExtendedAPI()
    {
        Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows " +
        "NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
        "Chrome/86.0.4240.198 Edg/86.0.622.69");
    }

    public async static Task<IDictionary<DateTime, WeatherData>> GetSkyConditionsExtended(string stationId, string state)
    {
        Func<string, int> parseToInt = (s) =>
        {
            return int.TryParse(s, out int i) ? i : 0;
        };

        Func<string, float> parseToFloat = (s) =>
        {
            return float.TryParse(s, out float f) ? f : 0;
        };


        var response = await MakeWebRequest(stationId, state);

        IList<int> cloudCover = new List<int>();
        IList<int> windDirectionDegrees = new List<int>();
        IList<int> windSpeedMph = new List<int>();
        IList<int> windGustMph = new List<int>();
        IList<int> cloudBaseFeet = new List<int>();
        IList<int> chanceOfLightning = new List<int>();
        IList<int> chanceOfPrecipitation = new List<int>();
        IList<int> chanceOfSnow = new List<int>();
        IList<int> dewPoint = new List<int>();

        IList<float> visibilityMiles = new List<float>();
        IList<float> temperatureFahrenheit = new List<float>();

        IList<string> flightCategory = new List<string>();

        Parallel.Invoke(
            () => cloudCover = GetWeatherData(response, "//tr[12]/td[@class='dbox']", parseToInt),
            () => windDirectionDegrees = GetWeatherData(response, "//tr[6]/td[@class='dbox']", parseToInt),
            () => windSpeedMph = GetWeatherData(response, "//tr[7]/td[@class='dbox']", parseToInt),
            () => windGustMph = GetWeatherData(response, "//tr[8]/td[@class='dbox']", parseToInt),
            () => cloudBaseFeet = GetWeatherData(response, "//tr[14]/td[@class='dbox']", parseToInt),
            () => chanceOfLightning = GetWeatherData(response, "//tr[22]/td[@class='dbox']", parseToInt),
            () => chanceOfPrecipitation = GetWeatherData(response, "//tr[23]/td[@class='dbox']", parseToInt),
            () => chanceOfSnow = GetWeatherData(response, "//tr[24]/td[@class='dbox']", parseToInt),
            () => dewPoint = GetWeatherData(response, "//tr[26]/td[@class='dbox']", parseToInt),

            () => visibilityMiles = GetWeatherData(response, "//tr[15]/td[@class='dbox']", parseToFloat),
            () => temperatureFahrenheit = GetWeatherData(response, "//tr[4]/td[@class='dbox']", parseToFloat),

            () => flightCategory = GetWeatherData(response, "//tr[16]/td[@class='cbox']", s => s)
        );

        DateTime date = DateTime.Now.Date;
        IDictionary<DateTime, WeatherData> weatherData = new Dictionary<DateTime, WeatherData>();

        var timeSlotTags = response.SelectNodes("//tr[2]/td[@class='tbox']");
        timeSlotTags.RemoveAt(0);

        DateTime hourMin = DateTime.ParseExact(timeSlotTags[0].InnerText, "h:mm tt", CultureInfo.InvariantCulture);
        date = date.AddHours(hourMin.Hour).AddMinutes(hourMin.Minute);

        for (int i = 0; i < timeSlotTags.Count; ++i)
        {
            weatherData.Add(date, new()
            {
                StationID = stationId,
                ObservationTime = date,
                WindDirectionDegrees = windDirectionDegrees[i],
                WindSpeedMph = windSpeedMph[i],
                WindGustMph = windGustMph[i],
                SkyConditions = new List<SkyConditions>()
                {
                    new () { CloudBaseFeetAGL = cloudBaseFeet[i], CloudCoverPercent = cloudCover[i] },
                },
                LightningPercent = chanceOfLightning[i],
                PrecipitationPercent = chanceOfPrecipitation[i],
                PrecipitationForSnowPercent = chanceOfSnow[i],
                DewPointFahrenheit = dewPoint[i],
                VisibilityStatuteMiles = visibilityMiles[i],
                TemperatureFahrenheit = temperatureFahrenheit[i],
                FlightCategory = flightCategory[i],
            });
            date = date.AddHours(3);
        }

        return weatherData;
    }

    private async static Task<HtmlNode> MakeWebRequest(string stationId, string state)
    {
        var response = await Client.GetAsync(WeatherObservationsGlobals.URL_FOR_US_AIRNET(stationId, state));
        HtmlDocument htmlDocument = new();
        htmlDocument.LoadHtml(await response.Content.ReadAsStringAsync());
        return htmlDocument.DocumentNode.SelectSingleNode("//table[@class='header']");
    }

    private static IList<T> GetWeatherData<T>(in HtmlNode response, in string xpath, in Func<string, T> parse)
    {
        var weatherDataTags = response.SelectNodes(xpath);
        IList<T> weatherData = new List<T>();
        foreach (var weatherDataTag in weatherDataTags)
        {
            weatherData.Add(parse(weatherDataTag.InnerText));
        }
        return weatherData;
    }

    private static HttpClient Client { get; } = new();
}