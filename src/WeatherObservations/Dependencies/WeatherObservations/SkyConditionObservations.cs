using System.Globalization;
using Amazon.DynamoDBv2.DataModel;
using HtmlAgilityPack;
using WeatherObservations.Data;
using WeatherObservations.Data.DynamoDB;
using WeatherObservations.Dependencies.Logger;

namespace WeatherObservations.Dependencies.WeatherObservations;

public class SkyConditionObservations : ISkyConditionObservations
{
    private ILogger Logger { get; init; }

    private IDynamoDBContext Context { get; init; }

    private HttpClient Client { get; init; }

    public SkyConditionObservations(ILogger logger, IDynamoDBContext dynamoDBContext)
    {
        this.Logger = logger;
        this.Context = dynamoDBContext;

        this.Client = new();
        this.Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows " +
        "NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
        "Chrome/86.0.4240.198 Edg/86.0.622.69");
        this.Client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        this.Client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
        this.Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        this.Client.DefaultRequestHeaders.Add("DNT", "1");
        this.Client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        this.Client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
    }

    public async Task<IDictionary<DateTime, WeatherData>> GetSkyConditionsAsync(string stationId, string state)
    {
        this.Logger.Log($"Getting Sky Conditions for {stationId} in {state}.");
        var stations = await this.Context.QueryAsync<WeatherData>(stationId).GetRemainingAsync();
        if (stations != null && stations.Count > 0)
        {
            this.Logger.Log($"Found {stations.Count} records for {stationId} in {state}.");
            DateTime firstRecorded = stations
                .Min(s => s.DateRecordedToDatabaseUtc)
                .AddHours(Configurations.DYNAMODB_HOURS_TO_KEEP_OBSERVATIONS);
            DateTime threshold = DateTime.UtcNow;
            if (DateTime.Compare(firstRecorded, threshold) < 0)
            {
                var deleteTasks = stations.Select(s => Context.DeleteAsync(s));
                await Task.WhenAll(deleteTasks);
            }
            else
            {
                return stations.ToDictionary(w => w.ObservationTimeLocal);
            }
        }

        Func<string, int> parseToInt = (s) =>
        {
            s = s.ToLower();
            if (s.Contains("k"))
            {
                s = s.Replace("k", "000");
            }
            return int.TryParse(s, out int i) ? i : 0;
        };

        Func<string, float> parseToFloat = (s) =>
        {
            return float.TryParse(s, out float f) ? f : 0;
        };

        this.Logger.Log($"No records found for {stationId} in {state}. Making web request.");
        var response = await MakeWebRequest(stationId, state);
        int utcOffset = int.Parse(response
            .SelectSingleNode("//span[@class='norm2']").InnerText
            .Split("UTC:")[1]
            .Split("&nbsp;&nbsp;")[0]
            .Trim());

        response = response.SelectSingleNode("//table[@class='header']");

        IList<int> cloudCover = new List<int>();
        IList<int> windDirectionDegrees = new List<int>();
        IList<int> windSpeedMph = new List<int>();
        IList<int> windGustMph = new List<int>();
        IList<int> cloudBaseFeet = new List<int>();
        IList<int> additionalCloudBaseFeet = new List<int>();
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
            () => additionalCloudBaseFeet = GetWeatherData(response, "//tr[13]/td[@class='dbox']", parseToInt),
            () => cloudBaseFeet = GetWeatherData(response, "//tr[14]/td[@class='dbox']", parseToInt),
            () => chanceOfLightning = GetWeatherData(response, "//tr[22]/td[@class='dbox']", parseToInt),
            () => chanceOfPrecipitation = GetWeatherData(response, "//tr[23]/td[@class='dbox']", parseToInt),
            // () => chanceOfSnow = GetWeatherData(response, "//tr[24]/td[@class='dbox']", parseToInt),
            () => dewPoint = GetWeatherData(response, "//tr[26]/td[@class='dbox']", parseToInt),

            () => visibilityMiles = GetWeatherData(response, "//tr[15]/td[@class='dbox']", parseToFloat),
            () => temperatureFahrenheit = GetWeatherData(response, "//tr[4]/td[@class='dbox']", parseToFloat),

            () => flightCategory = GetWeatherData(response, "//tr[16]/td[@class='cbox']", s => s)
        );


        var timeSlotTags = response.SelectNodes("//tr[2]/td[@class='tbox']");
        timeSlotTags.RemoveAt(0);

        // Snow Data is not available on this page.
        chanceOfSnow = Enumerable.Repeat(0, timeSlotTags.Count).ToList();


        DateTime hourMin = DateTime.ParseExact(timeSlotTags[0].InnerText, "h:mm tt", CultureInfo.InvariantCulture);
        DateTime dateLocalToStation = DateTime.UtcNow
            .AddHours(utcOffset).Date
            .AddHours(hourMin.Hour)
            .AddMinutes(hourMin.Minute);

        IDictionary<DateTime, WeatherData> weatherData = new Dictionary<DateTime, WeatherData>();
        for (int i = 0; i < timeSlotTags.Count; ++i)
        {
            List<SkyConditions> skyConditions = new();
            if (cloudBaseFeet[i] != 0)
            {
                skyConditions.Add(new() { CloudBaseFeetAGL = cloudBaseFeet[i], CloudCoverPercent = cloudCover[i] });
            }
            if (additionalCloudBaseFeet[i] != 0)
            {
                skyConditions.Add(new() {CloudBaseFeetAGL = additionalCloudBaseFeet[i], CloudCoverPercent = cloudCover[i] });
            }

            weatherData.Add(dateLocalToStation, new()
            {
                StationID = stationId,
                ObservationTimeUtc = dateLocalToStation.AddHours(-utcOffset),
                UtcOffset = utcOffset,
                DateRecordedToDatabaseUtc = DateTime.UtcNow,
                WindDirectionDegrees = windDirectionDegrees[i],
                WindSpeedMph = windSpeedMph[i],
                WindGustMph = windGustMph[i],
                SkyConditions = skyConditions,
                LightningPercent = chanceOfLightning[i],
                PrecipitationPercent = chanceOfPrecipitation[i],
                PrecipitationForSnowPercent = chanceOfSnow[i],
                DewPointFahrenheit = dewPoint[i],
                VisibilityStatuteMiles = visibilityMiles[i],
                TemperatureFahrenheit = temperatureFahrenheit[i],
                FlightCategory = flightCategory[i],
            });
            dateLocalToStation = dateLocalToStation.AddHours(3);
        }
        
        var saveTasks = weatherData.Values.Select(w => Context.SaveAsync(w));
        await Task.WhenAll(saveTasks);
        return weatherData;
    }

    private async Task<HtmlNode> MakeWebRequest(string stationId, string state)
    {
        var response = await this.Client.GetAsync(WeatherObservationsGlobals.URL_FOR_US_AIRNET(stationId, state));
        HtmlDocument htmlDocument = new();
        htmlDocument.LoadHtml(await response.Content.ReadAsStringAsync());

        // Select the body of the html document
        return htmlDocument.DocumentNode.SelectSingleNode("//body");
    }

    private IList<T> GetWeatherData<T>(in HtmlNode response, in string xpath, in Func<string, T> parse)
    {
        var weatherDataTags = response.SelectNodes(xpath);
        IList<T> weatherData = new List<T>();
        foreach (var weatherDataTag in weatherDataTags)
        {
            weatherData.Add(parse(weatherDataTag.InnerText));
        }
        return weatherData;
    }
}