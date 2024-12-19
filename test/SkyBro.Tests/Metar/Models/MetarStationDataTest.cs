using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class MetarStationDataTest
{
    private static string Path { get; } = "Metar/Models/TestData/MetarStationData";

    public static TheoryData<string, MetarStationData> TestCases => new()
    {
        {
            $"{Path}/MetarStationData1.json",
            new MetarStationData
            {
                Icao = "KRDU",
                Barometer = new Barometer
                {
                    Hg = 30.12M,
                    Hpa = 1020.0M,
                    Kpa = 102.0M,
                    Mb = 1019.98M,
                },
                Ceiling = new Ceiling
                {
                    Feet = 11000,
                    Meters = 3353
                },
                Clouds = [
                    new Cloud
                    {
                        BaseFeetAGL = 5000,
                        BaseMetersAGL = 1524,
                        Code = "FEW",
                        Text = "Few",
                        Feet = 5000,
                        Meters = 1524
                    },
                    new Cloud
                    {
                        BaseFeetAGL = 11000,
                        BaseMetersAGL = 3353,
                        Code = "BKN",
                        Text = "Broken",
                        Feet = 11000,
                        Meters = 3353
                    },
                    new Cloud
                    {
                        BaseFeetAGL = 20000,
                        BaseMetersAGL = 6096,
                        Code = "BKN",
                        Text = "Broken",
                        Feet = 20000,
                        Meters = 6096
                    },
                ],
                Conditions = [
                    new Condition()
                    {
                        Code = "OVC",
                        Text = "Overcast"
                    }
                ],
                Dewpoint = new Dewpoint
                {
                    Celsius = -5,
                    Fahrenheit = 23
                },
                Elevation = new Elevation
                {
                    Feet = 397.0M,
                    Meters = 121.0M
                },
                FlightCategory = "VFR",
                Humidity = new Humidity
                {
                    Percent = 32
                },
                Observed = "2024-12-01T18:51:00",
                Rain = new Rain()
                {
                    Inches = 0.01M,
                    Millimeters = 0.13M
                },
                Snow = new Snow
                {
                    Inches = 0.5M,
                    Millimeters = 12.7M
                },
                Station = new Station
                {
                    Geometry = new Geometry
                    {
                        Coordinates = new GeolocationCoordinate
                        {
                            Longitude = -78.78M,
                            Latitude = 35.9M
                        },
                        Type = "Point"
                    },
                    Location = "Raleigh, North Carolina, United States",
                    Name = "Raleigh-Durham International Airport",
                    Type = "Airport"
                },
                Temperature = new Temperature
                {
                    Celsius = 11,
                    Fahrenheit = 52
                },
                RawText = "KRDU 011851Z 22010KT 10SM FEW050 BKN110 BKN200 11/M05 A3012 RMK AO2 SLP202 T01061050",
                Visibility = new Visibility
                {
                    Miles = "Greater than 10",
                    MilesFloat = 10.0F,
                    Meters = "16,100",
                    MetersFloat = 16_100.0F
                },
                Wind = new Wind
                {
                    Degrees = 220,
                    SpeedKph = 19,
                    SpeedKts = 10,
                    SpeedMph = 12,
                    SpeedMps = 5
                }
            }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidDewpointData_ReturnsValidObject(string expectedDataFilePath, SkyBro.Metar.Models.MetarStationData expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<MetarStationData>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<MetarStationData>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Barometer?.Hg, actual: actualData?.Barometer?.Hg);
        Assert.Equal(expected: expectedData.Barometer?.Hpa, actual: actualData?.Barometer?.Hpa);
        Assert.Equal(expected: expectedData.Barometer?.Kpa, actual: actualData?.Barometer?.Kpa);
        Assert.Equal(expected: expectedData.Barometer?.Mb, actual: actualData?.Barometer?.Mb);

        Assert.Equal(expected: expectedData.Ceiling?.Feet, actual: actualData?.Ceiling?.Feet);
        Assert.Equal(expected: expectedData.Ceiling?.Meters, actual: actualData?.Ceiling?.Meters);

        Assert.Equal(expected: expectedData.Clouds?.Count(), actual: actualData?.Clouds?.Count());
        foreach (var (expectedCloud, actualCloud) in expectedData.Clouds?.Zip(actualData?.Clouds!)!)
        {
            Assert.Equal(expected: expectedCloud.BaseFeetAGL, actual: actualCloud.BaseFeetAGL);
            Assert.Equal(expected: expectedCloud.BaseMetersAGL, actual: actualCloud.BaseMetersAGL);
            Assert.Equal(expected: expectedCloud.Code, actual: actualCloud.Code);
            Assert.Equal(expected: expectedCloud.Feet, actual: actualCloud.Feet);
            Assert.Equal(expected: expectedCloud.Meters, actual: actualCloud.Meters);
            Assert.Equal(expected: expectedCloud.Text, actual: actualCloud.Text);
        }
        Assert.Equal(expected: expectedData.Conditions?.Count(), actual: actualData?.Conditions?.Count());
        foreach (var (expectedCondition, actualCondition) in expectedData?.Conditions?.Zip(actualData?.Conditions!)!)
        {
            Assert.Equal(expected: expectedCondition.Code, actual: actualCondition.Code);
            Assert.Equal(expected: expectedCondition.Text, actual: actualCondition.Text);
        }

        Assert.Equal(expected: expectedData.Dewpoint?.Celsius, actual: actualData?.Dewpoint?.Celsius);
        Assert.Equal(expected: expectedData.Dewpoint?.Fahrenheit, actual: actualData?.Dewpoint?.Fahrenheit);
        Assert.Equal(expected: expectedData.Elevation?.Feet, actual: actualData?.Elevation?.Feet);
        Assert.Equal(expected: expectedData.Elevation?.Meters, actual: actualData?.Elevation?.Meters);
        Assert.Equal(expected: expectedData.FlightCategory, actual: actualData?.FlightCategory);
        Assert.Equal(expected: expectedData.Humidity?.Percent, actual: actualData?.Humidity?.Percent);
        Assert.Equal(expected: expectedData.Icao, actual: actualData?.Icao);
        Assert.Equal(expected: expectedData.Observed, actual: actualData?.Observed);
        Assert.Equal(expected: expectedData.Rain?.Inches, actual: actualData?.Rain?.Inches);
        Assert.Equal(expected: expectedData.Rain?.Millimeters, actual: actualData?.Rain?.Millimeters);
        Assert.Equal(expected: expectedData.RawText, actual: actualData?.RawText);
        Assert.Equal(expected: expectedData.Snow?.Inches, actual: actualData?.Snow?.Inches);
        Assert.Equal(expected: expectedData.Snow?.Millimeters, actual: actualData?.Snow?.Millimeters);
        Assert.Equal(expected: expectedData.Station.Geometry.Coordinates.Latitude, actual: actualData?.Station.Geometry.Coordinates.Latitude);
        Assert.Equal(expected: expectedData.Station.Geometry.Coordinates.Longitude, actual: actualData?.Station.Geometry.Coordinates.Longitude);
        Assert.Equal(expected: expectedData.Station.Geometry.Type, actual: actualData?.Station.Geometry.Type);
        Assert.Equal(expected: expectedData.Station.Location, actual: actualData?.Station.Location);
        Assert.Equal(expected: expectedData.Station.Name, actual: actualData?.Station.Name);
        Assert.Equal(expected: expectedData.Temperature?.Celsius, actual: actualData?.Temperature?.Celsius);
        Assert.Equal(expected: expectedData.Temperature?.Fahrenheit, actual: actualData?.Temperature?.Fahrenheit);
        Assert.Equal(expected: expectedData.Visibility?.Meters, actual: actualData?.Visibility?.Meters);
        Assert.Equal(expected: expectedData.Visibility?.Miles, actual: actualData?.Visibility?.Miles);
        Assert.Equal(expected: expectedData.Visibility?.MilesFloat, actual: actualData?.Visibility?.MilesFloat);
        Assert.Equal(expected: expectedData.Visibility?.MetersFloat, actual: actualData?.Visibility?.MetersFloat);
        Assert.Equal(expected: expectedData.Wind?.Degrees, actual: actualData?.Wind?.Degrees);
        Assert.Equal(expected: expectedData.Wind?.GustKph, actual: actualData?.Wind?.GustKph);
        Assert.Equal(expected: expectedData.Wind?.GustKts, actual: actualData?.Wind?.GustKts);
        Assert.Equal(expected: expectedData.Wind?.GustMph, actual: actualData?.Wind?.GustMph);
        Assert.Equal(expected: expectedData.Wind?.GustMps, actual: actualData?.Wind?.GustMps);
        Assert.Equal(expected: expectedData.Wind?.SpeedKph, actual: actualData?.Wind?.SpeedKph);
        Assert.Equal(expected: expectedData.Wind?.SpeedKts, actual: actualData?.Wind?.SpeedKts);
        Assert.Equal(expected: expectedData.Wind?.SpeedMph, actual: actualData?.Wind?.SpeedMph);
        Assert.Equal(expected: expectedData.Wind?.SpeedMps, actual: actualData?.Wind?.SpeedMps);
    }
}