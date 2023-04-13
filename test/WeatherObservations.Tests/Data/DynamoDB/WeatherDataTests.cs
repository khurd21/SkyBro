using WeatherObservations.Data.DynamoDB;
using Xunit;

namespace WeatherObservations.Tests.Data.DynamoDB;

public class WeatherDataTests
{
    public static IEnumerable<object[]> FahrenheitToCelsiusConversions
    {
        get
        {
            yield return new object[] { 32, 0 };
            yield return new object[] { 212, 100 };
            yield return new object[] { 68, 20 };
        }
    }

    public static IEnumerable<object[]> UtcToLocalConversions
    {
        get
        {
            yield return new object[] { new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc), -8, new DateTime(2020, 12, 31, 16, 0, 0, DateTimeKind.Local) };
            yield return new object[] { new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Local) };
            yield return new object[] { new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc), 8, new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Local) };
        }
    }

    [Theory]
    [MemberData(nameof(FahrenheitToCelsiusConversions))]
    public void TestConversionFahrenheitToCelsius(float fahrenheit, float celsius)
    {
        var weatherData = new WeatherData()
        {
            TemperatureFahrenheit = fahrenheit,
        };

        Assert.Equal(celsius, weatherData.TemperatureCelsius);
        Assert.Equal(fahrenheit, weatherData.TemperatureFahrenheit);
    }

    [Theory]
    [MemberData(nameof(FahrenheitToCelsiusConversions))]
    public void TestConversionCelsiusToFahrenheit(float fahrenheit, float celsius)
    {
        var weatherData = new WeatherData()
        {
            TemperatureCelsius = celsius,
        };

        Assert.Equal(celsius, weatherData.TemperatureCelsius);
        Assert.Equal(fahrenheit, weatherData.TemperatureFahrenheit);
    }

    [Theory]
    [MemberData(nameof(UtcToLocalConversions))]
    public void TestUtcToLocalConversion(DateTime utcDateTime, int utcOffset, DateTime localDateTime)
    {
        var weatherData = new WeatherData()
        {
            ObservationTimeUtc = utcDateTime,
            UtcOffset = utcOffset,
        };

        Assert.Equal(localDateTime, weatherData.ObservationTimeLocal);
    }

    [Theory]
    [MemberData(nameof(FahrenheitToCelsiusConversions))]
    public void TestDewPointFahrenheitToCelsius(float fahrenheit, float celsius)
    {
        var weatherData = new WeatherData()
        {
            DewPointFahrenheit = fahrenheit,
        };

        Assert.Equal(celsius, weatherData.DewPointCelsius);
        Assert.Equal(fahrenheit, weatherData.DewPointFahrenheit);
    }

    [Theory]
    [MemberData(nameof(FahrenheitToCelsiusConversions))]
    public void TestDewPointCelsiusToFahrenheit(float fahrenheit, float celsius)
    {
        var weatherData = new WeatherData()
        {
            DewPointCelsius = celsius,
        };

        Assert.Equal(celsius, weatherData.DewPointCelsius);
        Assert.Equal(fahrenheit, weatherData.DewPointFahrenheit);
    }
}