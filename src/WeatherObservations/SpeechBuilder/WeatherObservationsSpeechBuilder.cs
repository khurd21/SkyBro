using WeatherObservations.Data;
using WeatherObservations.Data.DynamoDB;

namespace WeatherObservations.SpeechBuilder;

public class WeatherObservationsSpeechBuilder : SpeechBuilder
{
    private IList<WeatherData> Data { get; set; }

    public WeatherObservationsSpeechBuilder(IList<WeatherData> data) : base()
    {
        this.Data = data;
    }

    public WeatherObservationsSpeechBuilder() : this(new List<WeatherData>()) { }

    public WeatherObservationsSpeechBuilder ReportNoObservationsForAirport(string airport = "", DateTime date = default)
    {
        if (airport == string.Empty)
        {
            airport = "that airport";
        }
        this.Add($"I could not find any weather observations for {airport}");
        if (date != default)
        {
            this.Add($"on {date.ToString("dddd, MMMM d")}.");
        }
        else
        {
            this.Add(".");
        }
        return this;
    }

    public WeatherObservationsSpeechBuilder ReportFlightRules()
    {
        var weatherDataOnDay = Data
            .Where(x => x.ObservationTimeLocal.Hour >= Configurations.WEATHER_OBSERVATIONS_MINIMUM_TIME_FRAME)
            .Where(x => x.ObservationTimeLocal.Hour <= Configurations.WEATHER_OBSERVATIONS_MAXIMUM_TIME_FRAME);

        if (weatherDataOnDay.Count() == 0)
        {
            return this;
        }

        if (weatherDataOnDay.All(x => x.FlightCategory == weatherDataOnDay.First().FlightCategory))
        {
            this.Add($"Flight rules: {weatherDataOnDay.First().FlightCategory} all day.");
        }
        else
        {
            var mostCommonFlightRules = weatherDataOnDay
                .GroupBy(x => x.FlightCategory)
                .OrderByDescending(x => x.Count())
                .First()
                .Key;
            this.Add($"Flight rules: mostly {mostCommonFlightRules}.");
        }
        return this;
    }

    public WeatherObservationsSpeechBuilder ReportDate()
    {
        if (Data.FirstOrDefault()?.ObservationTimeLocal != null)
        {
            this.Add($"Date: {Data.FirstOrDefault()?.ObservationTimeLocal.ToString("dddd, MMMM d")}.");
        }
        return this;
    }

    public WeatherObservationsSpeechBuilder ReportIntroduction()
    {
        var station = Data.FirstOrDefault(x => x.StationID != null);
        if (station != null)
        {
            this.Add($"Weather observations for {station.StationID}.");
        }
        else
        {
            this.Add("SkyBro Reporting.");
        }
        return this;
    }

    public WeatherObservationsSpeechBuilder ReportAverageTemperature()
    {
        var avgTemp = Data.Average(x => x.TemperatureFahrenheit);
        if (avgTemp != null)
        {
            this.Add($"Average temperature: {Math.Round(avgTemp.GetValueOrDefault())} degrees Fahrenheit.");
        }
        return this;
    }

    public WeatherObservationsSpeechBuilder ReportWindConditions()
    {
        var maxWindSpeed = Data
            .Where(x => x.WindSpeedMph != null)
            .OrderByDescending(x => x.WindSpeedMph)
            .FirstOrDefault();
        var minWindSpeed = Data
            .Where(x => x.WindSpeedMph != null)
            .OrderBy(x => x.WindSpeedMph)
            .FirstOrDefault();

        if (maxWindSpeed == null || minWindSpeed == null)
        {
            return this;
        }
        if (maxWindSpeed.WindSpeedMph < Configurations.WEATHER_OBSERVATIONS_LIGHT_AND_VARIABLE_WIND_THRESHOLD)
        {
            this.Add($"Wind speed: light and variable.");
            return this;
        }
        if (maxWindSpeed.WindSpeedMph == minWindSpeed.WindSpeedMph)
        {
            this.Add($"Wind speed: {maxWindSpeed.WindSpeedMph} miles per hour.");
            return this;
        }

        bool isHigherWindSpeedLater = maxWindSpeed.ObservationTimeLocal > minWindSpeed.ObservationTimeLocal;
        string improvingOrWorking = isHigherWindSpeedLater ? "improving" : "working";
        var first = isHigherWindSpeedLater ? maxWindSpeed : minWindSpeed;
        var second = isHigherWindSpeedLater ? minWindSpeed : maxWindSpeed;

        this.Add($"Wind speed: {first.WindDirectionDegrees} degrees at {first.WindSpeedMph} miles per hour, " +
                        $"{improvingOrWorking} to {second.WindDirectionDegrees} degrees at {second.WindSpeedMph} miles per hour.");

        // Report gusts
        var maxWindGust = Data
            .Where(x => x.WindGustMph != null)
            .OrderByDescending(x => x.WindGustMph)
            .FirstOrDefault();

        if (maxWindGust?.WindGustMph > WeatherData.GUST_THRESHOLD)
        {
            this.Add($"Gusts up to {maxWindGust.WindGustMph}.");
        }

        return this;
    }

    public WeatherObservationsSpeechBuilder ReportPrecipitation()
    {
        var maxChanceOfRain = Data
            .Where(x => x.PrecipitationPercent != null)
            .Where(x => x.ObservationTimeLocal.Hour >= Configurations.WEATHER_OBSERVATIONS_MINIMUM_TIME_FRAME &&
                        x.ObservationTimeLocal.Hour <= Configurations.WEATHER_OBSERVATIONS_MAXIMUM_TIME_FRAME)
            .OrderByDescending(x => x.PrecipitationPercent)
            .FirstOrDefault();

        var minChanceOfRain = Data
            .Where(x => x.PrecipitationPercent != null)
            .Where(x => x.ObservationTimeLocal.Hour >= Configurations.WEATHER_OBSERVATIONS_MINIMUM_TIME_FRAME &&
                        x.ObservationTimeLocal.Hour <= Configurations.WEATHER_OBSERVATIONS_MAXIMUM_TIME_FRAME)
            .OrderBy(x => x.PrecipitationPercent)
            .FirstOrDefault();

        if (maxChanceOfRain == null || minChanceOfRain == null)
        {
            return this;
        }
        if (maxChanceOfRain.PrecipitationPercent == 0)
        {
            return this;
        }
        if (maxChanceOfRain.PrecipitationPercent == minChanceOfRain.PrecipitationPercent)
        {
            this.Add($"Chance of rain: {maxChanceOfRain.PrecipitationPercent} percent.");
            return this;
        }

        bool isHigherChanceOfRainLater = maxChanceOfRain.ObservationTimeLocal > minChanceOfRain.ObservationTimeLocal;
        string improvingOrWorking = isHigherChanceOfRainLater ? "working" : "improving";
        var first = isHigherChanceOfRainLater ? minChanceOfRain : maxChanceOfRain;
        var second = isHigherChanceOfRainLater ? maxChanceOfRain : minChanceOfRain;

        // I want the time to be spoken in 12 hour format, e.g. 7:00 AM or 7:00 PM
        this.Add($"Chance of rain: {first.PrecipitationPercent} percent at {first.ObservationTimeLocal.ToString("h tt")} " +
                        $"{improvingOrWorking} to {second.PrecipitationPercent} percent at {second.ObservationTimeLocal.ToString("h tt")}.");

        // If any of the observations have a chance of snow, report that as well
        var maxChanceOfSnow = Data
            .Where(x => x.PrecipitationForSnowPercent != null)
            .OrderByDescending(x => x.PrecipitationForSnowPercent)
            .FirstOrDefault();

        if (maxChanceOfSnow != null && maxChanceOfSnow.PrecipitationForSnowPercent > 0)
        {
            this.Add($"Chance of snow: {maxChanceOfSnow.PrecipitationForSnowPercent} percent " +
                            $"starting around {maxChanceOfSnow.ObservationTimeLocal.ToString("h tt")}.");
        }

        return this;
    }

    public WeatherObservationsSpeechBuilder ReportCloudConditions()
    {
        var orderedWeatherData = Data
            .Where(weatherData => weatherData.ObservationTimeLocal.Hour >= Configurations.WEATHER_OBSERVATIONS_MINIMUM_TIME_FRAME &&
                                  weatherData.ObservationTimeLocal.Hour <= Configurations.WEATHER_OBSERVATIONS_MAXIMUM_TIME_FRAME)
            .OrderByDescending(weatherData => weatherData.SkyConditions?
                .Max(skyConditions => skyConditions.CloudBaseFeetAGL));
        var maxCloudBaseObj = orderedWeatherData.FirstOrDefault();
        var minCloudBaseObj = orderedWeatherData.LastOrDefault();

        if (minCloudBaseObj == null || maxCloudBaseObj == null)
        {
            this.Add("Blue skies.");
            return this;
        }

        var maxCloudBase = maxCloudBaseObj.SkyConditions?
            .OrderByDescending(skyConditions => skyConditions.CloudBaseFeetAGL)
            .FirstOrDefault();

        var minCloudBase = minCloudBaseObj.SkyConditions?
            .OrderByDescending(skyConditions => skyConditions.CloudBaseFeetAGL)
            .LastOrDefault();

        if (maxCloudBase?.CloudBaseFeetAGL == minCloudBase?.CloudBaseFeetAGL)
        {
            this.Add($"Cloud base: {maxCloudBase?.CloudCoverPercent} percent coverage at " +
                            $"{maxCloudBase?.CloudBaseFeetAGL} feet.");
            return this;
        }
        if (maxCloudBaseObj.ObservationTimeLocal == minCloudBaseObj.ObservationTimeLocal)
        {
            this.Add($"Cloud base: {maxCloudBase?.CloudCoverPercent} percent coverage at " +
                            $"{maxCloudBase?.CloudBaseFeetAGL} feet.");
            return this;
        }

        bool isMaxCloudBaseLater = maxCloudBaseObj?.ObservationTimeLocal > minCloudBaseObj?.ObservationTimeLocal;
        string upOrDown = isMaxCloudBaseLater ? "ascending" : "descending";

        var firstWeatherData = isMaxCloudBaseLater ? minCloudBaseObj : maxCloudBaseObj;
        var firstSkyCondition = isMaxCloudBaseLater ? minCloudBase : maxCloudBase;
        var secondWeatherData = isMaxCloudBaseLater ? maxCloudBaseObj : minCloudBaseObj;
        var secondSkyCondition = isMaxCloudBaseLater ? maxCloudBase : minCloudBase;

        this.Add($"Cloud base: {firstSkyCondition?.CloudBaseFeetAGL} feet at " +
                        $"{firstWeatherData?.ObservationTimeLocal.ToShortTimeString()} with " +
                        $"{firstSkyCondition?.CloudCoverPercent} percent coverage, " +
                        $"{upOrDown} to {secondSkyCondition?.CloudBaseFeetAGL} feet " +
                        $"at {secondWeatherData?.ObservationTimeLocal.ToShortTimeString()}.");
        return this;
    }
}