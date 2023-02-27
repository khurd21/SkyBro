using Xunit;

namespace WeatherObservations.Tests;

public class AviationWeatherExtendedAPITest
{
    [Theory]
    [InlineData("KSHN", "WA")]
    public void TestGetSkyConditions(string stationId, string state)
    {
        Console.WriteLine("EXTENDED::");
        var response = Task.Run(async () => await AviationWeatherExtendedAPI.GetSkyConditionsExtended(stationId, state)).Result;
        Assert.Equal(response.Count, 22);
        foreach (var weather in response) {
            Assert.NotNull(weather.Value.FlightCategory);
            Assert.NotNull(weather.Value.ObservationTime);
            Assert.NotNull(weather.Value.TemperatureCelsius);
            Assert.NotNull(weather.Value.DewPointCelsius);
            Assert.NotNull(weather.Value.WindDirectionDegrees);
            Assert.NotNull(weather.Value.WindSpeedMph);
            Assert.NotNull(weather.Value.WindGustMph);
            Assert.NotNull(weather.Value.VisibilityStatuteMiles);
            Assert.NotNull(weather.Value.PrecipitationPercent);
            Assert.NotNull(weather.Value.PrecipitationForSnowPercent);
            Assert.NotNull(weather.Value.LightningPercent);
        }
    }
}