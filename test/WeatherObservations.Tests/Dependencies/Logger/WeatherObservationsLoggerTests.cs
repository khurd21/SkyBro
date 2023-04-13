using WeatherObservations.Dependencies.Logger;
using Xunit;

namespace WeatherObservations.Tests.Dependencies.Logger;

public class WeatherObservationsLoggerTests
{
    private WeatherObservationsLogger Logger { get; set; }

    public WeatherObservationsLoggerTests()
    {
        this.Logger = new();
    }

    [Fact]
    public void TestLog()
    {
        this.Logger.Log("Test");
    }
}