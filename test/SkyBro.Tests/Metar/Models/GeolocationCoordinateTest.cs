using System.Text.Json;

using Alexa.NET.Request;

using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests.Metar.Models;

public class GeolocationCoordinateTest
{
    private static string Path { get; } = "Metar/Models/TestData/GeolocationCoordinate";

    public static TheoryData<string, GeolocationCoordinate> TestCases => new()
    {
        {
            $"{Path}/GeolocationCoordinate1.json",
            new GeolocationCoordinate { Latitude = 40.72M, Longitude = -73.99M }
        },
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Serializer_WithValidGeoloationCoordinateData_ReturnsValidObject(string expectedDataFilePath, GeolocationCoordinate expectedData)
    {
        // Arrange
        var expectedDataJsonSchema = File.ReadAllText(expectedDataFilePath);

        // Act
        var actualData = JsonSerializer.Deserialize<GeolocationCoordinate>(expectedDataJsonSchema);
        var actualDataJsonSchema = JsonSerializer.Serialize(actualData);
        actualData = JsonSerializer.Deserialize<GeolocationCoordinate>(actualDataJsonSchema);

        // Assert
        Assert.Equal(expected: expectedData.Latitude, actual: actualData?.Latitude);
        Assert.Equal(expected: expectedData.Longitude, actual: actualData?.Longitude);
    }
}