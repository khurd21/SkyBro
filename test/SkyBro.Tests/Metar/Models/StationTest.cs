using System.Text.Json;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class StationTest
{
    private static string Path { get; } = "Metar/Models/TestData/Station";

    public static TheoryData<string, Station> TestCases => new()
    {
        {
            $"{Path}/Station1.json",
            new Station { Geometry = new Geometry { Coordinates = new GeolocationCoordinate { Longitude = -73.779317M, Latitude = 40.639447M }, Type = "Point" }, Location = "New York, New York, United States", Name = "John F Kennedy International Airport", Type = "Airport" }
        },
        {
            $"{Path}/Station2.json",
            new Station { Geometry = new Geometry { Coordinates = new GeolocationCoordinate { Longitude = -76.779317M, Latitude = 42.639447M }, Type = "Point"}, Location = "New York, New York, United States", Name = "John F Kennedy International Airport"}
        }
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidHumidityData_ReturnsValidObject(string expectedDataFilePath, Station expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<Station>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<Station>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Geometry.Coordinates.Latitude, actual: actualData?.Geometry.Coordinates.Latitude);
        Assert.Equal(expected: expectedData.Geometry.Coordinates.Longitude, actual: actualData?.Geometry.Coordinates.Longitude);
        Assert.Equal(expected: expectedData.Geometry.Type, actual: actualData?.Geometry.Type);
        Assert.Equal(expected: expectedData.Location, actual: actualData?.Location);
        Assert.Equal(expected: expectedData.Name, actual: actualData?.Name);
        Assert.Equal(expected: expectedData.Type, actual: actualData?.Type);
    }
}