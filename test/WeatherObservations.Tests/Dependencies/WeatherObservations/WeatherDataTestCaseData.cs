using WeatherObservations.Data.DynamoDB;

namespace WeatherObservations.Tests.Dependencies.WeatherObservations;

public static class WeatherDataTestCaseData
{
    public static List<WeatherData> GetOutdatedWeatherData()
    {
        return new()
        {
            new()
            {
                StationID = "KSEA",
                ObservationTimeUtc = DateTime.UtcNow.AddDays(-2),
                DateRecordedToDatabaseUtc = DateTime.UtcNow.AddDays(-2),
                UtcOffset = -8,
            },
        };
    }

    public static List<WeatherData> GetUpToDateWeatherData()
    {
        return new()
        {
            new()
            {
                StationID = "KSEA",
                ObservationTimeUtc = DateTime.UtcNow,
                DateRecordedToDatabaseUtc = DateTime.UtcNow,
                UtcOffset = -8,
            },
        };
    }
}