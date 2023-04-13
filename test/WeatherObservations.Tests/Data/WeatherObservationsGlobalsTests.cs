using WeatherObservations.Data;
using Xunit;

namespace WeatherObservations.Tests.Data;

public class WeatherObservationsGlobalsTests
{
    [Theory]
    [InlineData("KSHN", "WA")]
    public void TestUrlForUSAirnet(string stationId, string state)
    {
        string expectedBaseUrl = "https://www.usairnet.com";
        string expectedUrl = $"{expectedBaseUrl}/cgi-bin/launch/code.cgi?sta={stationId}&state={state}";

        Assert.Equal(expectedBaseUrl, WeatherObservationsGlobals.URL_FOR_US_AIRNET());
        Assert.Equal(expectedUrl, WeatherObservationsGlobals.URL_FOR_US_AIRNET(stationId, state));
    }

    [Fact]
    public void TestStationId()
    {
        Assert.Equal("station_id", WeatherObservationsGlobals.STATION_ID);
    }

    [Fact]
    public void TestRawText()
    {
        Assert.Equal("raw_text", WeatherObservationsGlobals.RAW_TEXT);
    }

    [Fact]
    public void TestObservationTime()
    {
        Assert.Equal("observation_time", WeatherObservationsGlobals.OBSERVATION_TIME);
    }

    [Fact]
    public void TestLatitude()
    {
        Assert.Equal("latitude", WeatherObservationsGlobals.LATITUDE);
    }

    [Fact]
    public void TestTemperatureCelsius()
    {
        Assert.Equal("temp_c", WeatherObservationsGlobals.TEMPERATURE_CELSIUS);
    }
    
    [Fact]
    public void TestDewpointCelsius()
    {
        Assert.Equal("dewpoint_c", WeatherObservationsGlobals.DEWPOINT_CELSIUS);
    }

    [Fact]
    public void TestWindDirectionDegrees()
    {
        Assert.Equal("wind_dir_degrees", WeatherObservationsGlobals.WIND_DIRECTION_DEGREES);
    }

    [Fact]
    public void TestWindSpeedKnots()
    {
        Assert.Equal("wind_speed_kt", WeatherObservationsGlobals.WIND_SPEED_KNOTS);
    }

    [Fact]
    public void TestWindGustKnots()
    {
        Assert.Equal("wind_gust_kt", WeatherObservationsGlobals.WIND_GUST_KNOTS);
    }

    [Fact]
    public void TestVisibilityStatuteMiles()
    {
        Assert.Equal("visibility_statute_mi", WeatherObservationsGlobals.VISIBILITY_STATUTE_MILES);
    }

    [Fact]
    public void TestAltimeterInHg()
    {
        Assert.Equal("altim_in_hg", WeatherObservationsGlobals.ALTIMETER_IN_HG);
    }

    [Fact]
    public void TestSeaLevelPressureMb()
    {
        Assert.Equal("sea_level_pressure_mb", WeatherObservationsGlobals.SEA_LEVEL_PRESSURE_MB);
    }

    [Fact]
    public void TestSkyConditions()
    {
        Assert.Equal("sky_condition", WeatherObservationsGlobals.SKY_CONDITIONS);
    }

    [Fact]
    public void TestSkyCover()
    {
        Assert.Equal("sky_cover", WeatherObservationsGlobals.SKY_COVER);
    }

    [Fact]
    public void TestCloudBaseFeetAgl()
    {
        Assert.Equal("cloud_base_ft_agl", WeatherObservationsGlobals.CLOUD_BASE_FEET_AGL);
    }

    [Fact]
    public void TestFlightCategory()
    {
        Assert.Equal("flight_category", WeatherObservationsGlobals.FLIGHT_CATEGORY);
    }

    [Fact]
    public void TestElevationMeters()
    {
        Assert.Equal("elevation_m", WeatherObservationsGlobals.ELEVATION_METERS);
    }

    [Fact]
    public void TestPrecipitationInches()
    {
        Assert.Equal("precip_in", WeatherObservationsGlobals.PRECIPITATION_INCHES);
    }
}