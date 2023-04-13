using WeatherObservations.Data.DynamoDB;

namespace WeatherObservations.Dependencies.WeatherObservations;

public interface ISkyConditionObservations
{
    Task<IDictionary<DateTime, WeatherData>> GetSkyConditionsAsync(string stationId, string state);
}

