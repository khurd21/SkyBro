using WeatherObservations.Data;

namespace WeatherObservations;

public class WeatherObservationsSpeechBuilder
{
    private IList<WeatherData> Data { get; set; }

    public string Speech { get; private set; }

    public WeatherObservationsSpeechBuilder(IList<WeatherData> data)
    {
        this.Data = data;
        this.Speech = string.Empty;
    }

    public WeatherObservationsSpeechBuilder()
    {
        this.Data = new List<WeatherData>();
        this.Speech = string.Empty;
    }

    public WeatherObservationsSpeechBuilder ReportNoObservationsForAirport(string airport = "")
    {
        if (airport == string.Empty)
        {
            airport = "that airport";
        }
        this.Speech += $"I'm sorry, I could not find any weather observations for {airport}.";
        return this;
    }

    public WeatherObservationsSpeechBuilder ReportIntroduction()
    {
        var station = this.Data.FirstOrDefault(x => x.StationID != null);
        if (station != null)
        {
            this.Speech += $"Weather observations for {station.StationID}. ";
        }
        else
        {
            this.Speech += "SkyBro Reporting. ";
        }
        return this;
    }

    public WeatherObservationsSpeechBuilder ReportAverageTemperature()
    {
        var avgTemp = this.Data.Average(x => x.TemperatureFahrenheit);
        if (avgTemp != null)
        {
            this.Speech += $"Average temperature: {Math.Round(avgTemp.GetValueOrDefault())} degrees Fahrenheit. ";
        }
        return this;
    }

    public WeatherObservationsSpeechBuilder ReportWindConditions()
    {
        var maxWindSpeed = this.Data
            .Where(x => x.WindSpeedMph != null)
            .OrderByDescending(x => x.WindSpeedMph)
            .FirstOrDefault();
        var minWindSpeed = this.Data
            .Where(x => x.WindSpeedMph != null)
            .OrderBy(x => x.WindSpeedMph)
            .FirstOrDefault();

        if (maxWindSpeed == null || minWindSpeed == null)
        {
            return this;
        }
        if (maxWindSpeed.WindSpeedMph == minWindSpeed.WindSpeedMph)
        {
            this.Speech += $"Wind speed: {maxWindSpeed.WindSpeedMph} miles per hour. ";
            return this;
        }

        bool isHigherWindSpeedLater = maxWindSpeed.ObservationTime > minWindSpeed.ObservationTime;
        string improvingOrWorking = isHigherWindSpeedLater ? "improving" : "working";
        var first = isHigherWindSpeedLater ? maxWindSpeed : minWindSpeed;
        var second = isHigherWindSpeedLater ? minWindSpeed : maxWindSpeed;

        this.Speech += $"Wind speed: {first.WindDirectionDegrees} degrees at {first.WindSpeedMph} miles per hour, " +
                        $"{improvingOrWorking} to {second.WindDirectionDegrees} degrees at {second.WindSpeedMph} miles per hour. ";

        // Report gusts
        var maxWindGust = this.Data
            .Where(x => x.WindGustMph != null)
            .OrderByDescending(x => x.WindGustMph)
            .FirstOrDefault();

        if (maxWindGust?.WindGustMph > WeatherData.GUST_THRESHOLD)
        {
            this.Speech += $"Gusts up to {maxWindGust.WindGustMph} miles per hour. ";
        }

        return this;
    }

    public WeatherObservationsSpeechBuilder ReportPrecipitation()
    {
        var maxChanceOfRain = this.Data
            .Where(x => x.PrecipitationPercent != null)
            .Where(x => x.ObservationTime.GetValueOrDefault().Hour >= 7 &&
                        x.ObservationTime.GetValueOrDefault().Hour <= 22)
            .OrderByDescending(x => x.PrecipitationPercent)
            .FirstOrDefault();

        var minChanceOfRain = this.Data
            .Where(x => x.PrecipitationPercent != null)
            .Where(x => x.ObservationTime.GetValueOrDefault().Hour >= 7 &&
                        x.ObservationTime.GetValueOrDefault().Hour <= 22)
            .OrderBy(x => x.PrecipitationPercent)
            .FirstOrDefault();

        if (maxChanceOfRain == null || minChanceOfRain == null)
        {
            return this;
        }
        if (maxChanceOfRain.PrecipitationPercent == minChanceOfRain.PrecipitationPercent)
        {
            this.Speech += $"Chance of rain: {maxChanceOfRain.PrecipitationPercent} percent. ";
            return this;
        }

        bool isHigherChanceOfRainLater = maxChanceOfRain.ObservationTime > minChanceOfRain.ObservationTime;
        string improvingOrWorking = isHigherChanceOfRainLater ? "improving" : "working";
        var first = isHigherChanceOfRainLater ? maxChanceOfRain : minChanceOfRain;
        var second = isHigherChanceOfRainLater ? minChanceOfRain : maxChanceOfRain;

        // I want the time to be spoken in 12 hour format, e.g. 7:00 AM or 7:00 PM
        this.Speech += $"Chance of rain: {first.PrecipitationPercent} percent at {first.ObservationTime?.ToString("h tt")} " +
                        $"{improvingOrWorking} to {second.PrecipitationPercent} percent at {second.ObservationTime?.ToString("h tt")}. ";
        
        // If any of the observations have a chance of snow, report that as well
        var maxChanceOfSnow = this.Data
            .Where(x => x.PrecipitationForSnowPercent != null)
            .OrderByDescending(x => x.PrecipitationForSnowPercent)
            .FirstOrDefault();
        
        if (maxChanceOfSnow != null)
        {
            this.Speech += $"Chance of snow: {maxChanceOfSnow.PrecipitationForSnowPercent} percent " +
                            $"starting around {maxChanceOfSnow.ObservationTime?.ToString("h tt")}. ";
        }

        return this;
    }

    public WeatherObservationsSpeechBuilder ReportCloudConditions()
    {
        var orderedWeatherData = this.Data
            .Where(weatherData => weatherData.ObservationTime.GetValueOrDefault().Hour >= 7 &&
                                  weatherData.ObservationTime.GetValueOrDefault().Hour <= 22)
            .OrderByDescending(weatherData => weatherData.SkyConditions?
                .Max(skyConditions => skyConditions.CloudBaseFeetAGL));
        var maxCloudBaseObj = orderedWeatherData.FirstOrDefault();
        var minCloudBaseObj = orderedWeatherData.LastOrDefault();

        if (minCloudBaseObj == null || maxCloudBaseObj == null)
        {
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
            this.Speech += $"Cloud base: {maxCloudBase?.CloudCoverPercent} percent coverage at " +
                            $"{maxCloudBase?.CloudBaseFeetAGL} feet. ";
            return this;
        }

        bool isMaxCloudBaseLater = maxCloudBaseObj?.ObservationTime > minCloudBaseObj?.ObservationTime;
        string upOrDown =  isMaxCloudBaseLater ? "ascending" : "descending";

        var firstWeatherData = isMaxCloudBaseLater ? minCloudBaseObj : maxCloudBaseObj;
        var firstSkyCondition = isMaxCloudBaseLater ? minCloudBase : maxCloudBase;
        var secondWeatherData = isMaxCloudBaseLater ? maxCloudBaseObj : minCloudBaseObj;
        var secondSkyCondition = isMaxCloudBaseLater ? maxCloudBase : minCloudBase;

        this.Speech += $"Cloud base: {firstSkyCondition?.CloudBaseFeetAGL} feet starting at " +
                        $"{firstWeatherData?.ObservationTime.GetValueOrDefault().Hour} with " +
                        $"{firstSkyCondition?.CloudCoverPercent} percent coverage, " +
                        $"{upOrDown} to {secondSkyCondition?.CloudBaseFeetAGL} feet " +
                        $"at {secondWeatherData?.ObservationTime.GetValueOrDefault().Hour}. " +
                        $"Cloud coverage: {firstSkyCondition?.CloudCoverPercent} percent coverage. ";

        return this;
    }
}